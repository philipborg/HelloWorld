namespace HelloWorld {
    class MyPartialToStandaloneHtmlProvider : PartialToStandaloneHtmlProvider {
        public MyPartialToStandaloneHtmlProvider() : base(@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <title>{0}</title>

    <script src=""/sys/webcomponentsjs/webcomponents.min.js""></script>
    <link rel=""import"" href=""/sys/polymer/polymer.html"">
    
    <link rel=""import"" href=""/sys/puppet-client/puppet-client.html"">
    <link rel=""import"" href=""/sys/imported-template/imported-template.html"">
    
    <link rel=""import"" href=""/sys/bootstrap.html"">
    <style>
        body {{
            margin: 20px;
        }}
    </style>

    <link rel=""import"" href=""/sys/starcounter-debug-aid/src/starcounter-debug-aid.html"">
</head>
<body>
    <template is=""dom-bind"" id=""puppet-root"">
        <template is=""imported-template"" content$=""{{{{model.Html}}}}"" model=""{{{{model}}}}""></template>
    </template>
    <puppet-client ref=""puppet-root""></puppet-client>
    <starcounter-debug-aid></starcounter-debug-aid>
</body>
</html>") {
        }
    }
}