$(document).ready(function () {
    let ctxOps = {
        selector: '.reg-context',
        trigger: 'left',
        callback: function (key, options) {
            let keyToFunc = {
                'edit': function (options) {
                    options.$trigger.find('.edit-link')[0].click()
                },
                'print': function (options) {
                    console.log('nyi')
                },
                'replacement': function (options) {
                    console.log('nyi')
                },
                'name-change': function (options) {
                    let $form = options.$trigger.find('.change-name-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_change_name_form'))
                    swal({
                        title: 'Change name form',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_change_name_form').submit()
                        }
                    })
                },
                'residency-change': function (options) {
                    let $form = options.$trigger.find('.change-residency-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_change_residency_form'))
                    swal({
                        title: 'Change residency form',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_change_residency_form').submit()
                        }
                    })
                },
                'deceased': function (options) {
                    let $form = options.$trigger.find('.deceased-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_deceased_form'))
                    swal({
                        title: 'Set Deceased Form',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_deceased_form').submit()
                        }
                        })
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
                'elector-transfer': function (options) {
                    let $form = options.$trigger.find('.transfer-elector-form')
                    let $form_parent = $form.parent()
                    $form.remove()
                    let $newForm = $('<div>').html($form.clone().removeClass('d-none').attr('id', 'dialog_elector_transfer_form'))
                    swal({
                        title: 'Transfer of elector form',
                        type: 'info',
                        html: $newForm.html(),
                        showCancelButton: true,
                        confirmButtonText: 'Apply'
                    }).then(function (result) {
                        $form_parent.append($form)
                        if (result.value) {
                            $('#dialog_elector_transfer_form').submit()
                        }
                    })
                },
            }
            keyToFunc[key](options)
        },
        items: {
            "edit": { name: "Edit", icon: "far fa-edit" },
            "print": { name: "Print ID" },
            "replacement": { name: "Administer Replacement" },
            "process": {
                "name": "Process Registration",
                "items": {
                    "name-change": { name: "Name Change" },
                    "residency-change": {name : "Change Residence" },
                    "deceased": { name: "Toggle Deceased" },
                    "disallowed": { name: "Toggle Disallowed" },
                    "disqualified": { name: "Toggle Disqualified" },
                    "elector-transfer": { name: "Transfer Elector" }
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