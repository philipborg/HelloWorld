using System;
using Starcounter;

namespace HelloWorld {
    public class REST {
        //optional. Puppet is better :)

        public static void CreateRestHandlers() {
            //curl -i -X GET http://127.0.0.1:8080/HelloWorld/likeables
            Handle.GET("/HelloWorld/likeables", () => {
                var likeables = Db.SQL<Likeable>("SELECT o FROM Likeable o");

                var json = new LikeablesJson();
                json.Likeables.Data = likeables;

                return json;
            });



            //curl -i -X POST http://127.0.0.1:8080/HelloWorld/like/...
            Handle.POST("/HelloWorld/like/{?}", (Request req, string likeableId) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE o.Key = ?", likeableId).First;

                if (likeable == null) {
                    return GetRestResponse(404, "Not Found", "Error: Likeable not found");
                }

                var token = Program.GetCurrentUserToken(req);
                var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", likeable, token).First;

                if (existing == null) {
                    Db.Transact(() => {
                        new Like() {
                            ToWhat = likeable,
                            UserToken = token
                        };
                    });
                    return GetRestResponse(200, "OK", "Like created succesfully");
                }
                else {
                    return GetRestResponse(403, "Forbidden", "Error: Like already exists for this token");
                }
            });



            //curl -i -X DELETE http://127.0.0.1:8080/HelloWorld/like/...
            Handle.DELETE("/HelloWorld/like/{?}", (Request req, string likeableId) => {
                var likeable = Db.SQL<Likeable>("SELECT o FROM Likeable o WHERE o.Key = ?", likeableId).First;

                if (likeable == null) {
                    return GetRestResponse(404, "Not Found", "Error: Likeable not found");
                }

                var token = Program.GetCurrentUserToken(req);
                var existing = Db.SQL<Like>("SELECT o FROM \"Like\" o WHERE o.ToWhat = ? AND o.UserToken = ?", likeable, token).First;

                if (existing != null) {
                    Db.Transact(() => {
                        existing.Delete();
                    });
                    return GetRestResponse(200, "OK", "Like deleted succesfully");
                }
                else {
                    return GetRestResponse(403, "Forbidden", "Error: Like that does not exist for this token");
                }
            });
        }


        static Response GetRestResponse(ushort status, string statusDescription, string body) {
            return new Response() {
                StatusCode = status,
                StatusDescription = statusDescription,
                Body = body
            };
        }
    }
}
