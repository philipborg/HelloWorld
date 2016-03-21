using System;
using Starcounter;

namespace HelloWorld {
    class Program {

        static void CreateData() {
            //dive into Database directory to see the data model

            Db.Transact(() => {
                //create
                var quote = new Quotation() {
                    Author = "Albert",
                    Text = "Everything should be made as simple as possible, but not simpler."
                };
                var likeable = new Likeable();
                likeable.Quotation = quote;


                //read
                Likeable likeableAlbert = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE Quotation.Author = ?", "Albert").First;
                Quotation quotation = likeableAlbert.Quotation;


                //update
                quotation.Author += " Einstein";
            });


            //go to http://localhost:8181/#/databases/default/sql to browse the data in Administrator
        }



        static void DeleteData() {
            //delete
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM \"Like\""); //with SQL
                Db.SlowSQL("DELETE FROM Likeable");
            });

            var quotations = Db.SQL<Quotation>("SELECT q FROM Quotation q");
            foreach (var item in quotations) {
                Db.Transact(() => {
                    item.Delete(); //with code
                });
            }
        }



        static void Main() {

            //create test data
            //DeleteData();
            var any = Db.SQL<Likeable>("SELECT o FROM Likeable o").First;
            if (any == null) {
                CreateData();
            }


            //create REST handlers. Only when you really need a REST API, otherwise just go on to use Puppet
            REST.CreateRestHandlers();


            //create Puppet handler
            //this is the entry point for localhost:8080/HelloWorld in your browser
            Handle.GET("/HelloWorld", (Request req) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o FETCH ?", 1).First;

                var page = new LikeableQuotationPage() {
                    Data = likeable,
                    UserToken = GetCurrentUserToken(req)
                };

                return page;
            });

        }

        public static string GetCurrentUserToken(Request req) {
            return req.ClientIpAddress.ToString();
        }
    }
}