﻿using System;
using Starcounter;
using System.Collections.Generic;
using Simplified.Ring1;
using Simplified.Ring2;

namespace HelloWorld
{
    [Database]
    public class Spender : Person
    {
        public QueryResultRows<Expense> Spendings
            => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);

        public decimal CurrentBalance
            => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }

    [Database]
    public class Expense : Something
    {
        public Spender Spender;
        public decimal Amount;
    }

    class Program
    {
        static void Main()
        {
            var anyone = Db.SQL<Spender>("SELECT s FROM Spender s").First;
            if (anyone == null)
            {
                Db.Transact(() =>
                {
                    new Spender()
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                });
            }

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () =>
            {
                return Db.Scope(() =>
                {
                    var person = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                    var json = new PersonJson()
                    {
                        Data = person
                    };
                    json.Session = new Session(SessionOptions.PatchVersioning);

                    var expenses = person.Spendings;
                    foreach (var expense in expenses)
                    {
                        var expenseJson = Self.GET("/HelloWorld/partial/expense/" + expense.GetObjectID());
                        json.Expenses.Add(expenseJson);
                    }

                    return json;
                });
            });

            Handle.GET("/HelloWorld/partial/expense/{?}", (string id) =>
            {
                var json = new ExpenseJson();
                json.Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id));
                return json;
            });
        }
    }
}