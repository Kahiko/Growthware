<!DOCTYPE html>
<html>
<head>
    <meta content="" charset="utf-8" />
    <title>@ViewData("Title")</title>
    <link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    <script src="@Url.Content("~/Scripts/jquery-1.5.1.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/modernizr-1.7.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/GW.NavigationControler.js")" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">


        function InitPageNavigation() {
            var mNavControler = new NavigationControler();
            var mRefreshObject = new RefreshObject();
            mRefreshObject.ContentAreaID = 'HMenu';
            mRefreshObject.Refresh = function refresh() {
                // make call to either get data or html
                var mRetHTML = "hierarchical menu Refresh from javascript!!!";
                // if data only retruned then bind
                // the data to a template
                // put the results into the UI
                $('#' + this.ContentAreaID).html(mRetHTML.toString());
            }
            mNavControler.RegisterRefreshObject(mRefreshObject);

            var mRefreshObject = new RefreshObject();
            mRefreshObject.ContentAreaID = 'title';
            mRefreshObject.Refresh = function refresh() {
                // make call to either get data or html
                var mRetHTML = "<h1>My MVC Application Refresh from javascript!!!</h1>";
                // if data only retruned then bind
                // the data to a template
                // put the results into the UI
                $('#' + this.ContentAreaID).html(mRetHTML.toString());
            }
            mNavControler.RegisterRefreshObject(mRefreshObject);
        }

        function Refresh() {
            var mNavControler = new NavigationControler();
            mNavControler.Refresh();
        }

        function Load(action) {
            var mNavControler = new NavigationControler();
            mNavControler.Load(action);
        }

        $(document).ready(function () {
            InitPageNavigation()
        });
    </script>
</head>
<body>
    <div class="page">
        <header>
            <div id="title">
                <h1>My MVC Application</h1>
            </div>
            <div id="logindisplay">
                @Html.Partial("_LogOnPartial")
            </div>
            <nav>
                <ul id="menu">
                    <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    <li>@Html.ActionLink("About", "About", "Home")</li>
                    @If Request.IsAuthenticated Then
                       @:<li> @Html.ActionLink("Functions", "Index", "Functions") </li>
                    End If
                </ul>
            </nav>
        </header>
        <section>
            <div id="HMenu">
                hierarchical menu
            </div>
            <input type="button" id="button" onclick="javascript:Refresh();" value="Refresh" />
            <input type="button" id="button2" onclick="javascript:Load('myAction');" value="Load" />
            @RenderBody()
        </section>
        <footer>
        </footer>
    </div>
</body>
</html>
