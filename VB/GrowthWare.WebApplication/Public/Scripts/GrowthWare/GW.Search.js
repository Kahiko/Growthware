/// <reference path="GW.Model.js" />

// Namespaces
if (typeof GW == "undefined" || !GW) {
    window.GW = { name: 'Growthware Core Web', version: '1.0.0.0' };
};

GW.Search = {

    DefaultImageSrc: function () {
        return "Public/Images/GrowthWare/sort_asc.png";
    },

    Criteria: { SelectedPage: 1,
        PageSize: 10,
        Columns: '',
        OrderByColumn: '',
        OrderByDirection: 'ASC',
        WhereClause: '"1 = 1"'
    },

    SearchColumn: {},

    RecordsReturned: {},

    $TotalPages: {},

    $DropSelectPage: {},

    $CurrentPage: {},

    SearchResultsContainer: {},

    URL: {},

    URLParameters: "",

    SortImage: {},

    GetQueryOptions: function () {
        var mRetVal = '';
        for (var key in this.Criteria) {
            if (this.Criteria.hasOwnProperty(key)) {
                mRetVal = mRetVal + key + '=' + escape(this.Criteria[key]) + '&';
            }
        }
        mRetVal = mRetVal.substr(0, (mRetVal.length - 1)); // remove the last &
        return mRetVal;
    },

    NextPage: function () { this.MovePage('plus', 1); },
    PreviousPage: function () { this.MovePage('minus', 1); },
    FirstPage: function () { this.MovePage('minus', 0); },
    LastPage: function () { this.MovePage('plus', 0); },

    MovePage: function (direction, count) {
        var mCurrentSelectedPage = this.Criteria.SelectedPage;
        var mTotalPage = parseInt(this.$TotalPages.html().toString());
        if (direction == 'plus') {
            if (this.Criteria.SelectedPage < mTotalPage) {
                if (count == 1) {
                    this.Criteria.SelectedPage = parseInt(this.Criteria.SelectedPage) + 1;
                } else {
                    this.Criteria.SelectedPage = mTotalPage;
                }
            }
        } else {
            if (count == 1) {
                if (this.Criteria.SelectedPage > 1) {
                    this.Criteria.SelectedPage = parseInt(this.Criteria.SelectedPage) - 1;
                } else {
                    this.Criteria.SelectedPage = 1;
                }
            } else {
                this.Criteria.SelectedPage = 1;
            }
        }
        if (mCurrentSelectedPage != this.Criteria.SelectedPage) {
            this.GetSearchResults();
        }
    },

    GetSearchResults: function () {
        var defaultOptions = GW.Model.DefaultWebMethodOptions();
        var mOptions = $.extend({}, defaultOptions, this.CallWebOptions);
        mOptions.url = this.URL + this.GetQueryOptions() + this.URLParameters
        GW.Common.JQueryHelper.callWeb(mOptions, this.GetResultsSuccess, this.GetResultsError);
    },

    GetResultsSuccess: function (xhr) {
        GW.Search.SearchResultsContainer.empty();
        var $searchResultsData = {};
        if ($('#searchResultsData').length > 0) {
            $searchResultsData = $('#searchResultsData');
        } else {
            $searchResultsData = $('<div id="searchResultsData"></div>');
            $searchResultsData.appendTo(GW.Search.SearchResultsContainer);
        }
        $searchResultsData.html(xhr);

        GW.Search.UpdateUI();
    },

    GetResultsError: function (xhr, status, error) {
        mRetHTML = "Status: " + status + " " + "Error: " + error;
        GW.Common.debug(mRetHTML);
        GW.Search.SearchResultsContainer.html(mRetHTML);
    },

    UpdateUI: function () {
        var mTotalPages = Math.ceil(this.RecordsReturned / this.Criteria.PageSize);
        if (mTotalPages == 0) mTotalPages = 1;
        if (GW.Search.Criteria.SelectedPage == NaN) {
            GW.Search.Criteria.SelectedPage = 1;
        }
        GW.Search.BuildDropSelectPage(mTotalPages);
        this.$CurrentPage.html(this.Criteria.SelectedPage);
        this.$TotalPages.html(mTotalPages);
        this.$DropSelectPage.val(this.Criteria.SelectedPage);
    },

    BuildDropSelectPage: function (numToBuild) {
        var dropSelectPageLength = this.$DropSelectPage.find('option').length;
        if (dropSelectPageLength == 0 || dropSelectPageLength != numToBuild) {
            this.$DropSelectPage.empty();
            for (var i = 1; i < (numToBuild + 1) ; i++) {
                this.$DropSelectPage.append($("<option />").val(i).text(i));
            }
            this.$DropSelectPage.bind('click', GW.Search.onDropSelectPageChanged);
        }
    },

    onRecordsChanged: function ($txtBox) {
        var mRecordsPerPage = $txtBox.value
        GW.Search.Criteria.PageSize = mRecordsPerPage;
        GW.Search.Criteria.SelectedPage = 1;
        GW.Search.$DropSelectPage.empty();
        GW.Search.GetSearchResults();
        return true;
    },

    onDropSelectPageChanged: function () {
        var selectedValue = GW.Search.$DropSelectPage.find(':selected').val();
        if (GW.Search.Criteria.SelectedPage != selectedValue) {
            GW.Search.Criteria.SelectedPage = selectedValue;
            GW.Search.GetSearchResults();
        }
        return true;
    },

    toggleSort: function (sortControl) {
        var sortColumn = sortControl.name.replace("headerSort", "");
        var src = GW.Search.DefaultImageSrc();
        var direction = ""
        if (GW.Search.Criteria.OrderByDirection.toLowerCase() == "asc") {
            direction = "desc"
            src = src.replace("asc", "desc");
        } else {
            direction = "asc"
            src = src.replace("desc", "asc");
        }
        GW.Search.Criteria.OrderByDirection = direction;
        GW.Search.Criteria.OrderByColumn = sortColumn;
        GW.Search.SearchColumn = sortColumn;
        GW.Search.GetSearchResults();
    },

    setSortImage: function () {
    	$("[name~='headerSort']").each(function () {
    		$(this).attr("src", "Public/Skins/Blue Arrow/Images/Spacer.gif");
    	});
    	this.SortImage = document.getElementById('imgSort' + this.SearchColumn);
    	if (!this.SortImage) {
    		return;
    	}
    	var src = GW.Search.DefaultImageSrc();
    	if (GW.Search.Criteria.OrderByDirection.toLowerCase() != "asc") {
    		src = src.replace("asc", "desc");
    	}
    	this.SortImage.src = src;
    },

    init: function () {
        this.SearchResultsContainer = $('#searchResults');
        this.$DropSelectPage = $('#ddSelectPage')
        this.$TotalPages = $('#totalPages');
        this.$CurrentPage = $('#SearchControl_currentPage');
        GW.Search.Criteria.PageSize = $('#SearchControl_txtRecordsPerPage').val();
    }
}
