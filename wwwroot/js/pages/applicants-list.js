$(document).ready(function () {
    let ctxOps = {
        selector: '.reg-context',
        trigger: 'left',
        callback: function (key, options) {
            let keyToFunc = {
                'edit': function (options) {
                    options.$trigger.find('.edit-link')[0].click()
                },
                'disallowed': function (options) {
                    let $form = options.$trigger.find('.disallowed-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_disallowed_form'))
                    swal({
                        title: 'Confirm Toggling Disallowed',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_disallowed_form').submit()
                        }
                        })
                },
                'disqualified': function (options) {
                    let $form = options.$trigger.find('.disqualified-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_disqualified_form'))
                    swal({
                        title: 'Confirm Toggling Disqualification',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_disqualified_form').submit()
                        }
                    })
                },
            }
            keyToFunc[key](options)
        },
        items: {
            "edit": { name: "Edit", icon: "far fa-edit" },
            "process": {
                "name": "Process Registration",
                "items": {
                    "disallowed": { name: "Toggle Disallowed" },
                    "disqualified": { name: "Toggle Disqualified" }

                }
            }
        }
    }

    $.contextMenu(ctxOps)
    $.contextMenu(function () {
        let thisCtxOps = ctxOps;
        thisCtxOps.trigger = 'right'
        thisCtxOps.selector = '.reg-item'
        return thisCtxOps;
    }())
    
})