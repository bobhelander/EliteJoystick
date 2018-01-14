using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder.Exploration
{
    public static class Page
    {
        public static string Text = @"<!DOCTYPE html>
<html lang = ""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head><meta charset = ""utf-8"" /><title></title>
<link rel=""stylesheet"" href=""styles.css"">
</head>
<body>
{0}
</body>
</html>";



        public static string Entry = @"<div class=""row"">
<div class=""column"">
 <p>{0}</p>
</div>
<div class=""column"">
 <p>{1}</p>
</div>
<div class=""column distance"">
 <p>{2}</p>
</div>
<div class=""column"">
 <p>{3}</p>
</div>
</div>";
    }
}
