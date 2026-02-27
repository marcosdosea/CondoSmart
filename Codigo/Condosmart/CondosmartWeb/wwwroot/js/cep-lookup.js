// cep-lookup.js
// Ao preencher o campo CEP (8 dígitos) faz validação via endpoint /Cep/GetEndereco
$(function() {
    var $cep = $("[name='Cep']");
    if (!$cep.length) return;

    function setField(name, value) {
        var v = value || '';
        var $byName = $("[name='" + name + "']");
        if ($byName.length) { $byName.val(v); return true; }
        var $byId = $("#" + name);
        if ($byId.length) { $byId.val(v); return true; }
        var $byNameSuffix = $("[name$='." + name + "']");
        if ($byNameSuffix.length) { $byNameSuffix.val(v); return true; }
        return false;
    }

    function createWarning(message, onOverwrite, onMerge) {
        // remove existing
        $("#cep-address-warning").remove();
        var $div = $("<div id='cep-address-warning' class='alert alert-warning d-flex justify-content-between align-items-center' role='alert'></div>");
        var $msg = $("<div></div>").text(message + ' ');
        var $btns = $("<div class='d-flex gap-2'></div>");
        var $merge = $("<button type='button' class='btn btn-sm btn-outline-secondary'>Mesclar vazios</button>");
        var $overwrite = $("<button type='button' class='btn btn-sm btn-primary'>Sobrescrever</button>");
        var $close = $("<button type='button' class='btn-close' aria-label='Fechar'></button>");

        $merge.on('click', function() { if (onMerge) onMerge(); $div.remove(); });
        $overwrite.on('click', function() { if (onOverwrite) onOverwrite(); $div.remove(); });
        $close.on('click', function() { $div.remove(); });

        $btns.append($merge).append($overwrite);
        $div.append($msg).append($btns).append($close);

        // insert at top of form
        var $form = $("form.row.g-3");
        if ($form.length) $form.prepend($div);
        else $("body").prepend($div);
    }

    function normalize(s) {
        if (!s) return '';
        return String(s).trim().toLowerCase();
    }

    var timeout = null;
    $cep.on('input', function() {
        clearTimeout(timeout);
        var val = $(this).val() || '';
        val = ('' + val).replace(/\D/g,'');
        if (val.length < 8) return; // aguarda completar
        timeout = setTimeout(function() {
            $.getJSON('/Cep/GetEndereco?cep=' + val)
                .done(function(data) {
                    if (!data) return;

                    var viaRua = data.rua || '';
                    var viaBairro = data.bairro || '';
                    var viaCidade = data.cidade || '';
                    var viaUf = (data.uf || '').toUpperCase();

                    // merge only empty fields by default
                    if (!$("[name='Rua']").val()) setField('Rua', viaRua);
                    if (!$("[name='Bairro']").val()) setField('Bairro', viaBairro);
                    if (!$("[name='Cidade']").val()) setField('Cidade', viaCidade);
                    if (!$("[name='Uf']").val()) setField('Uf', viaUf);
                    if (!$("[name='Complemento']").val()) setField('Complemento', data.complemento || '');

                    // compare with selected condominium address, if any
                    var condId = $("select[name='CondominioId']").val();
                    var condo = (window.condominiosData || []).find(function(d){ return String(d.id) === String(condId); });
                    if (condo) {
                        var diff = false;
                        if (normalize(condo.Rua || condo.rua) !== normalize(viaRua)) diff = true;
                        if (normalize(condo.Bairro || condo.bairro) !== normalize(viaBairro)) diff = true;
                        if (normalize(condo.Cidade || condo.cidade) !== normalize(viaCidade)) diff = true;
                        if (normalize((condo.Uf || condo.uf) || '') !== normalize(viaUf)) diff = true;

                    if (diff) {
                            var defaultMsg = 'Endereco retornado pelo CEP e diferente do endereco do condominio selecionado. Deseja sobrescrever ou mesclar?';
                            createWarning(window.cepWarningMessage || defaultMsg,
                                function() {
                                    // overwrite all address fields (keep Numero)
                                    setField('Rua', viaRua);
                                    setField('Bairro', viaBairro);
                                    setField('Cidade', viaCidade);
                                    setField('Uf', viaUf);
                                    setField('Complemento', data.complemento || '');
                                },
                                function() {
                                    // merge: already merged defaults, just keep
                                }
                            );
                        }
                    }
                })
                .fail(function(jqxhr, textStatus, err) {
                    console.warn('CEP lookup failed', textStatus, err);
                    var msg = 'Erro ao validar CEP.';
                    if (jqxhr && jqxhr.status === 404) msg = 'CEP não encontrado.';
                    else if (jqxhr && jqxhr.status === 400) msg = 'CEP inválido.';
                    showCepError(msg);
                });
        }, 300);
    });
    
    function showCepError(message) {
        clearCepError();
        $cep.addClass('is-invalid');
        var $err = $("<div class='invalid-feedback' id='cep-error'></div>").text(message);
        $cep.after($err);
    }

    function clearCepError() {
        $cep.removeClass('is-invalid');
        $("#cep-error").remove();
    }
});
