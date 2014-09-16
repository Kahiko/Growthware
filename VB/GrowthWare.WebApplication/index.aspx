<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Public/Skins/Default/Default.Master" CodeBehind="index.aspx.vb" Inherits="GrowthWare.WebApplication.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        // Declares the NG application.  Pass any external application within the bracketts
        var app = angular.module('main', []);

        // defines the service
        angular.module('main').factory("pricingService", ["$http", function ($http) {
            return {
                getProducts: function () {
                    return $http.get('api/products', { responseType: 'json' });
                },
                getProductByID: function (id) {
                    var p = {
                        id: id
                    };
                    return $http.get('api/products', { params: p, responseType: 'json' });
                }
            };
        }]);

        // defines the controllers and calls the services
        angular.module('main').controller("pricingController", ["$scope", "pricingService", function ($scope, pricingService) {
            $scope.xyz = "abc";
            $scope.getProducts = function () {
                pricingService.getProducts().success(function (data) {
                    if (data) {
                        $scope.Products = data;
                    }
                });
            }
            $scope.getProducts();
        }]);

        angular.module('main').controller("productController", ["$scope", "pricingService", function ($scope, pricingService) {
            $scope.getProduct = function () {
                pricingService.getProductByID($scope.productId)
                .then(function (response) {
                    if (response.data) {
                        $scope.Product = response.data;
                    }
                },
                function () {
                    $scope.Product.Name = "Invalid ID";
                }
                );
            };
        }]);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <body ng-app="main">
        <div ng-controller="pricingController">
            <h2>All Products</h2>
            <ul id="products">
                <li ng-repeat="item in Products">{{item.Name}}</li>
            </ul>

            <%--{{Products}}--%>
        </div>
        <div ng-controller="productController">
            <h2>Search by ID</h2>
            <input type="text" ng-model="productId" id="prodId" size="5" />
            <input type="button" value="Search" ng-click="getProduct();" />
            <div>{{Product.Name}}</div> 
        </div>


        <script type="text/javascript" language="javascript">

        </script>
    </body>
</asp:Content>
