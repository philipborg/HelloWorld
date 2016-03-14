using System;
using Starcounter;

namespace HelloWorld {
    class Program {
        static void CreateData() {
            //delete (with SQL)
            Db.Transact(() => {
                Db.SlowSQL("DELETE FROM \"Like\"");
                Db.SlowSQL("DELETE FROM Likeable");
            });

            //delete (with code)
            var quotations = Db.SQL<Quotation>("SELECT q FROM Quotation q");
            foreach (var quotation in quotations) {
                Db.Transact(() => {
                    quotation.Delete();
                });
            }

            //create
            Db.Transact(() => {
                var quotation = new Quotation() {
                    Author = "Albert",
                    Text = "Wszystko powinno być tak proste, jak to tylko możliwe, ale nie prostsze."
                };

                var likeable = new Likeable();
                likeable.What = quotation;
            });


            {
                //read (with SQL)
                Likeable likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE What.Author = ?", "Albert").First;

                //read (with code)
                Quotation quotation = likeable.What;

                //update
                Db.Transact(() => {
                    quotation.Author += " Einstein";
                });
            }
        }

        static string GetCurrentUserToken(Request req) {
            return req.ClientIpAddress.ToString();
        }

        static void Main() {

            CreateData();

            //curl -i -X GET http://127.0.0.1:8080/HelloWorld/likeables
            Handle.GET("/HelloWorld/likeables", () => {
                var likeables = Db.SQL<Likeable>("SELECT o FROM Likeable o");

                var json = new LikeablesJson();
                json.Likeables.Data = likeables;

                return json;
            });

            //curl -i -X POST http://127.0.0.1:8080/HelloWorld/like/XXX
            Handle.POST("/HelloWorld/like/{?}", (Request req, string likeableId) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE o.Key = ?", likeableId).First;

                if (likeable == null) {
                    return new Response() {
                        StatusCode = 404,
                        StatusDescription = "Not Found",
                        Body = "Error: Likeable not found"
                    };
                }

                var token = GetCurrentUserToken(req);
                var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", likeable, token).First;

                if (existing == null) {
                    Db.Transact(() => {
                        new Like() {
                            ToWhat = likeable,
                            UserToken = token
                        };
                    });
                    return new Response() {
                        StatusCode = 200,
                        Body = "Like created succesfully"
                    };
                }
                else {
                    return new Response() {
                        StatusCode = 403,
                        StatusDescription = "Forbidden",
                        Body = "Error: Like already exists for this token"
                    };
                }
            });

            //curl -i -X POST http://127.0.0.1:8080/HelloWorld/like/XXX
            Handle.DELETE("/HelloWorld/like/{?}", (Request req, string likeableId) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE o.Key = ?", likeableId).First;

                if (likeable == null) {
                    return new Response() {
                        StatusCode = 404,
                        StatusDescription = "Not Found",
                        Body = "Error: Likeable not found"
                    };
                }

                var token = GetCurrentUserToken(req);
                var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", likeable, token).First;

                if (existing != null) {
                    Db.Transact(() => {
                        existing.Delete();
                    });
                    return new Response() {
                        StatusCode = 200,
                        Body = "Like deleted succesfully"
                    };
                }
                else {
                    return new Response() {
                        StatusCode = 403,
                        StatusDescription = "Forbidden",
                        Body = "Error: Like that does not exist for this token"
                    };
                }
            });


            Handle.GET("/HelloWorld", (Request req) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o FETCH ?", 1).First;

                var page = new LikeableQuotationPage() {
                    Data = likeable,
                    UserToken = GetCurrentUserToken(req)
                };

                if (Session.Current != null) {
                    page.Session = Session.Current;
                }
                else {
                    page.Session = new Session(SessionOptions.PatchVersioning);
                }

                return page;
            });

        }
    }
}