using Starcounter;

namespace HelloWorld
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;
    }

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
                if (anyone == null)
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());

            string HTML = @"<!doctype html>
                            <html lang=""en"">
                                <head>
                                    <meta charset=""utf-8"">
                                    <script src=""https://rawgit.com/alshakero/Palindrom/master/dist/palindrom.min.js""></script>
                                    <link rel=""import"" href=""HelloWorld/sys/polymer/polymer-element.html"">
                                    <title>Polymer 2.0 HelloWorld</title>
                                </head>
                                <style>
                                    body {{
                                        margin: 20px;
                                        color: #444
                                    }}
                                </style>
                                <body>
                                    <dom-module id=""my-element"">
                                        <template>
                                            <style>
                                                div {{
                                                    margin: 5px
                                                }}
                    
                                                input {{
                                                    font-size: 1.2em;
                                                    padding: 5px;
                                                    color: #444
                                                }}
                                            </style>
                                            <h1>{{{{model.FullName}}}}</h1>
                                            <div>
                                                <label>First name:</label>
                                                <input value=""{{{{model.FirstName$::input}}}}"">
                                            </div>
                                            <div>
                                                <label>Last name:</label>
                                                <input value=""{{{{model.LastName$::input}}}}"">
                                            </div>
                                            <button value=""{{{{model.Save$::click}}}}"" onmousedown=""++this.value"">Save</button>
                                            <button value=""{{{{model.Cancel$::click}}}}"" onmousedown=""++this.value"">Cancel</button>
                                        </template>
                                        <script> 
                                        class MyElement extends Polymer.Element {{
                                            static get is() {{ return 'my-element'; }}
                                            static get config() {{
                                                return {{
                                                    properties: {{
                                                        model: {{
                                                            type: Object
                                                        }}
                                                    }}
                                                }}
                                            }}
                                            constructor() {{
                                                super();
                                            }}
                                            connectedCallback() {{
                                                super.connectedCallback();
                                                let that = this;
                                                let defaultLocalVersionPath = ""/_ver#c$"";
                                                let defaultRemoteVersionPath = ""/_ver#s"";
                                                let palindrom = new Palindrom({{
                                                    remoteUrl: ""http://localhost:8080/HelloWorld"", ot: true, localVersionPath: defaultLocalVersionPath,
                                                    remoteVersionPath: defaultRemoteVersionPath, useWebSocket: true, callback: function (obj) {{
                                                        that.model = obj;
                                                    }}
                                                }});
                                            }}
                                        }}
                                        // Register custom element definition using standard platform API
                                        customElements.define(MyElement.is, MyElement);
                                        </script>
                                    </dom-module>
                                    <my-element></my-element>
                                </body>
                            </html>";
            Application.Current.Use(new PartialToStandaloneHtmlProvider(HTML));

            Handle.GET("/HelloWorld", () =>
            {
                return Db.Scope(() =>
                {
                    var person = Db.SQL<Person>("SELECT p FROM Person p").First;
                    var json = new PersonJson()
                    {
                        Data = person
                    };

                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }
                    json.Session = Session.Current;

                    return json;
                });
            });
        }
    }
}