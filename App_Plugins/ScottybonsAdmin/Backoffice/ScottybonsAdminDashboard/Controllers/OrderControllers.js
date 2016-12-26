
angular.module('umbraco').controller('OrderController', ['$scope', '$http', function ($scope, $http) {

    //$scope.myAppScopeProvider = {
    //    showInfo: function (row) {
    //        var modalInstance = $modal.open({
    //            controller: 'ChildCtrl',
    //            templateUrl: 'ngTemplate/infoPopup.html',
    //            resolve: {
    //                selectedRow: function () {
    //                    return row.entity;
    //                }
    //            }
    //        });

    //        modalInstance.result.then(function (selectedItem) {
    //            $log.log('modal selected Row: ' + selectedItem);
    //        }, function () {
    //            $log.info('Modal dismissed at: ' + new Date());
    //        });
    //    }
    //}

   

    $scope.gridOptions = {

        showFooter: true,
        enableSorting: true,
        multiSelect: false,
        enableFiltering: true,
        enableRowSelection: true,
        enableSelectAll: false,
        enableRowHeaderSelection: false,
        selectionRowHeaderWidth: 35,
        noUnselect: true,
        enableGridMenu: true,

        columnDefs: [
                   { field: 'OrderId', width: '15%', displayName: 'Order Number' },
                   { field: 'CreatedOn', width: '10%', displayName: 'Order Date' },
                   { field: 'CustomerID', width: '10%', displayName: 'Customer ID' },
                   { field: 'FirstName', width: '15%', displayName: 'First Name' },
                   { field: 'LastName', width: '15%', displayName: 'Last Name' },
                   { field: 'ContactNumber', width: '15%', displayName: 'Phone Number' },
                   { field: 'OrderStatusName', width: '20%', displayName: 'Order Status' }
        ],


        exporterCsvFilename: 'OrderList.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [15, 15, 15, 15] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'blue' },
        exporterPdfHeader: { text: "Order List", style: 'headerStyle' },
        exporterPdfFooter: function (currentPage, pageCount) {
            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        },
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 20, bold: true };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
        },
        exporterPdfOrientation: 'landscape',
        exporterPdfPageSize: 'LETTER',
        exporterPdfMaxGridWidth: 500,
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),

        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        },
        paginationPageSize: 20,
        paginationPageSizes: [20, 40, 60, 80],
        //appScopeProvider: $scope.myAppScopeProvider,
        rowTemplate: "<div ng-dblclick=\"grid.appScope.showInfo(row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>"


    };


    $scope.paginationGridOptions = {
        paginationPageSizes: [20, 40, 60, 80],
        paginationPageSize: 20,
    };
    angular.extend($scope.paginationGridOptions, $scope.gridOptions);

    var apiOrderListUrl = "/Umbraco/Api/OrderWebApi/GetOrderList";

    $http.get(apiOrderListUrl)
        .success(function (data) {
        debugger;
            data.forEach(function (row) {
                row.registered = Date.parse(row.registered);
            });
            $scope.gridOptions.data = data;
            $scope.paginationGridOptions.data = data;
           
        }).error(function () {
            $scope.title = "Oops... something went wrong";
            $scope.working = false;
        });

    angular.extend($scope.paginationGridOptions, $scope.gridOptions);
}]);


//angular.module('umbraco').controller('ChildCtrl', ['$scope', '$http', 'uiGridConstants', '$interval', '$modal', '$modalInstance', '$log', 'selectedRow', function ($scope, $http, uiGridConstants, $interval, $modal, $modalInstance, $log, selectedRow) {

//    $scope.selectedRow = selectedRow;

//    var orderId = $scope.selectedRow.OrderId;
//    var excelName = "orderdetailsOf" + orderId + ".csv";
//    //var statusTemplate = '<div>{{getExternalScopes().showMe(row.entity.SecondColumnValue,row.entity.AnswerImage)}}</div>';
//    var statusTemplate1 = '<div>{{COL_FIELD != null ? "<img ng-src=data:image/JPEG;base64,COL_FIELD>" : ""}}</div>';
//    $scope.gridOptions = {
//        showFooter: false,
//        enableSorting: false,
//        multiSelect: false,
//        enableFiltering: false,
//        enableRowSelection: false,
//        enableSelectAll: false,
//        enableRowHeaderSelection: false,
//        exporterMenuCsv: true,

//        enableGridMenu: true,
//        noUnselect: true,

//        headerTemplate: '<div class="ui-grid-top-panel" style="text-align: center">Order Details for : ' + orderId + '</div>',
//        columnDefs: [

//            { field: 'FirstColumnValue', displayName: 'Name', width: '65%', },
//            { field: 'SecondColumnValue', displayName: 'Value', width: '35%', }
//            //{
//            //    field: 'AnswerImage',
//            //    displayName: 'Image',
//            //    cellTemplate: statusTemplate1 //'<img ng-src="data:image/JPEG;base64,{{COL_FIELD}}">'
//            //}
//        ],



//        exporterCsvFilename: excelName,

//        exporterPdfDefaultStyle: { fontSize: 9 },
//        exporterPdfTableStyle: { margin: [30, 15, 15, 15] },
//        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'blue' },
//        exporterPdfHeader: { text: "Customer Order Details ", style: 'headerStyle' },
//        exporterPdfFooter: function (currentPage, pageCount) {
//            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
//        },
//        exporterPdfCustomFormatter: function (docDefinition) {
//            docDefinition.styles.headerStyle = { fontSize: 20, bold: true };
//            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
//            return docDefinition;
//        },
//        exporterPdfOrientation: 'landscape',
//        exporterPdfPageSize: 'LETTER',
//        exporterPdfMaxGridWidth: 500,
//        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),

//        onRegisterApi: function (gridApi) {
//            $scope.gridApi = gridApi;
//        },



//    };


//    var apiOrderDetailsUrl = "/Umbraco/Api/OrderWebApi/GetOrderDetailsById/" + orderId;
//    $http.get(apiOrderDetailsUrl)
//        .success(function (data) {
//            data.forEach(function (row) {
//                row.registered = Date.parse(row.registered);
//            });
//            $scope.gridOptions.data = data;
//        }).error(function () {
//            $scope.title = "Oops... something went wrong";
//            $scope.working = false;
//        });

//    $scope.getTemplate = function (value) {
//        if (value != null) {
//            return '<img ng-src="data:image/JPEG;base64,{{value}}">';
//        }
//        return "";
//    };


//    $scope.ok = function () {
//        $scope.selectedRow = null;
//        $modalInstance.close();
//    };

//    $scope.cancel = function () {
//        $scope.selectedRow = null;
//        $modalInstance.dismiss('cancel');
//    };


//}]);


 