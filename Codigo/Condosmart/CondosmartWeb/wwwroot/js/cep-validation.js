// Validação de CEP para formulários
$(function() {
    console.debug('cep-validation.js loaded');
    
    var $cepInput = $("input[name='Cep']");
    
    if ($cepInput.length === 0) {
        console.debug('CEP input not found');
        return;
    }
    
    console.debug('CEP input found - initializing CEP validation');
    
    function clearEnderecoFields() {
        $("input[name='Rua']").val('');
        $("input[name='Numero']").val('');
        $("input[name='Bairro']").val('');
        $("input[name='Complemento']").val('');
        $("input[name='Cidade']").val('');
        $("input[name='Uf']").val('');
    }

    function showCepError(message) {
        var $alert = $(".cep-error-alert");
        if ($alert.length === 0) {
            $alert = $('<div class="alert alert-danger alert-dismissible fade show cep-error-alert" role="alert"></div>');
            $cepInput.after($alert);
        }
        $alert.html(message + '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>');
        $alert.show();
    }

    function hideCepError() {
        $(".cep-error-alert").fadeOut(300, function() {
            $(this).hide();
        });
    }

    function validateAndFillCep(cep) {
        // Remove non-digits
        var cleanCep = cep.replace(/\D/g, '');
        
        // Validate length
        if (cleanCep.length !== 8) {
            clearEnderecoFields();
            showCepError('CEP deve conter 8 dígitos.');
            return;
        }

        // Fetch from API
        $.ajax({
            url: '/Cep/GetEndereco?cep=' + encodeURIComponent(cleanCep),
            method: 'GET',
            dataType: 'json',
            success: function(data) {
                console.debug('CEP validation result:', data);
                if (data) {
                    $("input[name='Rua']").val(data.rua || '');
                    $("input[name='Bairro']").val(data.bairro || '');
                    $("input[name='Complemento']").val(data.complemento || '');
                    $("input[name='Cidade']").val(data.cidade || '');
                    $("input[name='Uf']").val(data.uf || '');
                    $("input[name='Cep']").val(data.cep || '');
                    hideCepError();
                }
            },
            error: function(jqxhr, textStatus, error) {
                console.warn('Erro ao validar CEP:', textStatus, error);
                clearEnderecoFields();
                if (jqxhr.status === 404) {
                    showCepError('CEP não encontrado. Verifique o CEP informado.');
                } else if (jqxhr.status === 503) {
                    showCepError('Serviço de CEP indisponível. Tente novamente mais tarde.');
                } else {
                    showCepError('Erro ao validar CEP. Tente novamente.');
                }
            }
        });
    }

    // Event listener for CEP input
    $cepInput.on('blur', function() {
        var cep = $(this).val();
        if (cep && cep.trim() !== '') {
            validateAndFillCep(cep);
        }
    });
});
