$(document).ready(function () {
    const $pagination_select = $('#pagination_select')
    const $pagination_form_search = $('#pagination_form_search')
    const current_size = $pagination_select.attr('data-pagination-size')
    const current_page = $pagination_select.attr('data-pagination-page')
    var pageSizes = ['10', '30', '50', '100']
    var $option = $('<option>')
    // Make some default sizes to pick from
    var sizeInList = false;
    for (var sizeIdx in pageSizes) {
        let size = pageSizes[sizeIdx]
        if (current_size === size) {
            $pagination_select.append($option.clone().val(size).html(size).attr('selected', true))
            sizeInList = true
        }
        else {
            $pagination_select.append($option.clone().val(size).html(size))
        }
    }
    if (!sizeInList) {
        // Add the size we currently have
        $pagination_select.prepend($option.clone().val(current_size).html(current_size).attr('selected', true))
    }

    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

    // Handle size changes by the user
    $pagination_select.on('change', function () {
        let ret = '?page=' + current_page + "&size=" + $(this).val() + "&query=" + $pagination_select.attr('data-pagination-query')
        let urlVars = getUrlVars()
        for (var key in urlVars) {
            if (key == 'page' || key == 'size' || key == 'query') {
                continue
            }
            ret += '&' + key + '=' + urlVars[key]
        }
        window.location = ret
    })

    $pagination_form_search.on('submit', function (e) {        
        let ret = '?page=' + current_page + "&size=" + current_size + "&query=" + $pagination_form_search.find('input').val()
        let urlVars = getUrlVars()
        for (var key in urlVars) {
            if (key == 'page' || key == 'size' || key == 'query') {
                continue
            }
            ret += '&' + key + '=' + urlVars[key]
        }
        window.location = ret
        return false
    })

    $('.page-link-single').each(function (idx, elem) {
        let ret = '?page=' + $(this).attr('data-pagination-page') + "&size=" + current_size + "&query=" + $pagination_select.attr('data-pagination-query')
        let urlVars = getUrlVars()
        for (var key in urlVars) {
            if (key == 'page' || key == 'size' || key == 'query') {
                continue
            }
            ret += '&' + key + '=' + urlVars[key]
        }
        $(this).attr('href', ret)
    })

    $('.page-link-prev').each(function (idx, elem) {
        let ret = '?page=' + (parseInt(current_page) - 1) + "&size=" + current_size + "&query=" + $pagination_select.attr('data-pagination-query')
        let urlVars = getUrlVars()
        for (var key in urlVars) {
            if (key == 'page' || key == 'size' || key == 'query') {
                continue
            }
            ret += '&' + key + '=' + urlVars[key]
        }
        $(this).attr('href', ret)
    })

    $('.page-link-next').each(function (idx, elem) {
        let ret = '?page=' + (parseInt(current_page) + 1) + "&size=" + current_size + "&query=" + $pagination_select.attr('data-pagination-query')
        let urlVars = getUrlVars()
        for (var key in urlVars) {
            if (key == 'page' || key == 'size' || key == 'query') {
                continue
            }
            ret += '&' + key + '=' + urlVars[key]
        }
        $(this).attr('href', ret)
    })
})