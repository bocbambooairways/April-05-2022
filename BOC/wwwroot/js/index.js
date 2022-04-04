window.onload = function () {
    const searchMode = ko.observable('contains');
    const viewModel = {
        treeViewOptions: {
            items: models_BOC,
            width: 500,
            dataStructure: 'plain',
            parentIdExpr: 'categoryId',
            keyExpr: 'ID',
            displayExpr: 'name',
            width: 300, searchEnabled: true,
            searchMode, onItemClick(e) {
                const item = e.itemData;
              
                    switch (item.name) {
                        case "a.Flight view for Business Plan":
                            window.location.href = "/Lounge/Airport";
                            break;
                        case "b.Flight view (Quick view)":
                            window.location.href = "/FlightDate/Index";
                            break;
                        case "a.Load Factor for Station":
                            window.location.href = "/Report/Report1";
                            break;
                        case "a.Document Read of Pilot":
                            window.location.href = "/FO_DOC_READER/Index";
                            break;
                        case "b.Profile Baggage Found":
                            window.location.href="/BagFound/Index";
                            break;
                        case "c.Cài đặt BOC trên iOS":
                            window.open("https://testflight.apple.com/join/SAElif2I", "_blank");
                            break;
                        case "d.Input baggage miss":
                            /*Lưu localStorage token của staff*/
                            var station_token = $('#token').val();
                            localStorage.setItem('Token', station_token);
                            window.location.href = "/BaggageMiss?UserCurrent=Yes";
                            break;
                        case "03.Search":
                            window.location.href = "/Home/Search";
                            break;
                        case "04.Folder List":
                            window.location.href = "/Home/Folder";
                            break;
                }
            },
        },
        searchModeOptions: {
            dataSource: ['contains', 'startsWith'],
            value: searchMode,
        },
    };
    ko.applyBindings(viewModel);
};
