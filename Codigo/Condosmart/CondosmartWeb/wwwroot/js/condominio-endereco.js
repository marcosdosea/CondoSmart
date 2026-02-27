// Preenche campos de endere�o da unidade residencial ao selecionar um condom�nio
$(function() {
    console.debug('condominio-endereco.js loaded');
    var $select = $("select[name='CondominioId']");
    if ($select.length === 0) {
        console.debug('select [name=CondominioId] not found');
        return;
    }

    function setField(name, value) {
        var v = value || '';
        var $byName = $("[name='" + name + "']");
        if ($byName.length) {
            $byName.val(v);
            return true;
        }
        var $byId = $("#" + name);
        if ($byId.length) {
            $byId.val(v);
            return true;
        }
        // try suffix match (for possible model binding prefixes)
        var $byNameSuffix = $("[name$='." + name + "']");
        if ($byNameSuffix.length) {
            $byNameSuffix.val(v);
            return true;
        }
        return false;
    }

    function fetchAndFill(id) {
        if (!id) return;
        console.debug('Fetching endereco for condominio id=', id);
        $.getJSON('/Condominio/GetEndereco/' + id)
            .done(function(data) {
                console.debug('GetEndereco result:', data);
                if (!data) return;

                setField('Rua', data.rua);
                setField('Numero', data.numero);
                setField('Bairro', data.bairro);
                setField('Complemento', data.complemento);
                setField('Cidade', data.cidade);
                setField('Uf', data.uf);
                setField('Cep', data.cep);
            })
            .fail(function(jqxhr, textStatus, error) {
                console.warn('N�o foi poss�vel obter o endere�o do condom�nio.', textStatus, error);
                if (jqxhr && jqxhr.responseText) console.debug('response:', jqxhr.responseText);
            });
    }

    $select.on('change', function() {
        var id = $(this).val();
        console.debug('Condominio select changed, id=', id);
        fetchAndFill(id);
    });

    // if a condominium is already selected on load, fetch its address
    var initial = $select.val();
    if (initial) {
        console.debug('Condominio already selected on load, id=', initial, ' -> fetching address');
        fetchAndFill(initial);
    }
});
