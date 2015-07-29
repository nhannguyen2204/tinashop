function SetupPositionImageTile(con) {
    var conHeight = con.height();
    var image = con.children("img").first();
    var imgHeight = image.height();
    var gap = (imgHeight - conHeight) / 2;
    image.css("margin-top", -gap);
}

function GenerateActionsForProductListing() {
    $(".product-price-filter-action").click(function (e) {
        $('#LoadingModal').modal('show');
        var url = $(this).attr('data-url');

        var urlModel = {
            FromPrice: $("#minVal").val(),
            ToPrice: $("#maxVal").val(),
            ColorKeys: $(this).attr("data-color"),
            BrandCode: $(this).attr("data-brand"),
            CatCode: $(this).attr("data-cat"),
            IsOrderDatetimeDesc: $(this).attr("data-sortdate"),
            IsOrderPriceDesc: $(this).attr("data-sortprice")
        }

        setTimeout(function () {
            if (url == undefined) {
                $.ajax({
                    url: '/Products/GetSortUrlByModel',
                    type: 'post',
                    dataType: 'json',
                    success: function (data) {
                        if (!data.IsError) {
                            GetProductListing(data.Result);
                            window.history.pushState('filter-action', 'URL: ' + data.Result, data.Result);
                        }
                    },
                    data: urlModel
                });
            }
            else {
                GetProductListing(url);
                window.history.pushState('filter-action', 'URL: ' + url, url);
            }
        }, 500);
        e.preventDefault();
    });
}

function GetProductListing(url) {
    $.ajax({
        url: url,
        type: 'get',
        success: function (data) {
            $('#LoadingModal').modal('hide');
            $('.shop-filters').html('');
            $("#product-listing-container").html(data);

            GenerateActionsForProductListing();

            if (true) {
                //Price Slider Range
                var $minVal = parseInt($('#minVal').attr('data-min-val'));
                var $maxVal = parseInt($('#maxVal').attr('data-max-val'));
                var $startMin = parseInt($('#minVal').val());
                var $startMax = parseInt($('#maxVal').val());
                if ($('#price-range').length > 0) {
                    $('#price-range').noUiSlider({
                        range: {
                            'min': $minVal,
                            'max': $maxVal
                        },
                        start: [$startMin, $startMax],
                        connect: true,
                        serialization: {
                            lower: [
                                $.Link({
                                    target: $('#minVal'),
                                    format: {
                                        decimals: 0
                                    }
                                })
                            ],
                            upper: [
                                $.Link({
                                    target: $('#maxVal'),
                                    format: {
                                        decimals: 0
                                    }
                                })
                            ]
                        }
                    });
                }
            }

            if ($(window).width() <= 768) {
                $('.shop-filters').appendTo($('#filterModal .modal-body'));
                $('#filterModal .modal-body .shop-filters').css('display', 'block');
                setTimeout(function () {
                    if ($("#filterModal").attr("aria-hidden") == "false") {
                        $("body").addClass("modal-open");
                    }
                }, 500);
            }

            $(".product-tile-container img").each(function () {
                $(this).on("load", function () {
                    SetupPositionImageTile($(this).parent());
                });
            });

            $(".product-tile-container img").each(function () {
                SetupPositionImageTile($(this).parent());
            });
        }
    });
}

function TinaInit() {
    $(".product-tile-container img").each(function () {
        $(this).on("load", function () {
            SetupPositionImageTile($(this).parent());
        });
    });

    $(".product-tile-container img").each(function () {
        SetupPositionImageTile($(this).parent());
    });

    GenerateActionsForProductListing();

    window.addEventListener('popstate', function (event) {
        $('#LoadingModal').modal('show');

        setTimeout(function () {
            GetProductListing(location.pathname);
        }, 500);
    }, false);
}

$(document).ready(function (e) {
    TinaInit();
});