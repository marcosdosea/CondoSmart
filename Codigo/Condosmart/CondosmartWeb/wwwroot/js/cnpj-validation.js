// Validação de CNPJ para formulários
$(function() {
    console.debug('cnpj-validation.js loaded');
    
    var $cnpjInput = $("input[name='Cnpj']");
    
    if ($cnpjInput.length === 0) {
        console.debug('CNPJ input not found');
        return;
    }
    
    console.debug('CNPJ input found - initializing CNPJ validation');
    
    function clearEnderecoFields() {
        $("input[name='Rua']").val('');
        $("input[name='Numero']").val('');
        $("input[name='Bairro']").val('');
        $("input[name='Complemento']").val('');
        $("input[name='Cidade']").val('');
        $("input[name='Uf']").val('');
        $("input[name='Cep']").val('');
    }

    function showCnpjError(message) {
        var $alert = $(".cnpj-error-alert");
        if ($alert.length === 0) {
            $alert = $('<div class="alert alert-danger alert-dismissible fade show cnpj-error-alert" role="alert"></div>');
            $cnpjInput.after($alert);
        }
        $alert.html(message + '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>');
        $alert.show();
    }

    function hideCnpjError() {
        $(".cnpj-error-alert").fadeOut(300, function() {
            $(this).hide();
        });
    }

    function validateCnpjFormat(cnpj) {
        // Função mantida para compatibilidade, mas agora apenas faz validação básica
        var cleanCnpj = cnpj.replace(/\D/g, '');
        return cleanCnpj.length === 14;
    }

    function validateAndFillCnpj(cnpj) {
        // Remove non-digits
        var cleanCnpj = cnpj.replace(/\D/g, '');
        
        // Validate minimum length only
        if (cleanCnpj.length < 14) {
            clearEnderecoFields();
            showCnpjError('CNPJ deve conter 14 dígitos.');
            return;
        }

        // Take only first 14 digits if more were pasted
        cleanCnpj = cleanCnpj.substring(0, 14);
        
        console.debug('Validating CNPJ:', cleanCnpj);

        // Fetch from API - let the server validate the CNPJ
        $.ajax({
            url: '/api/Cnpj/consultar/' + encodeURIComponent(cleanCnpj),
            method: 'GET',
            dataType: 'json',
            timeout: 10000,
            success: function(data) {
                console.debug('CNPJ validation result:', data);
                if (data && data.valorValido) {
                    $("input[name='Rua']").val(data.rua || '');
                    $("input[name='Numero']").val(data.numero || '');
                    $("input[name='Bairro']").val(data.bairro || '');
                    $("input[name='Complemento']").val(data.complemento || '');
                    $("input[name='Cidade']").val(data.cidade || '');
                    $("input[name='Uf']").val(data.uf || '');
                    $("input[name='Cep']").val(data.cep || '');
                    hideCnpjError();
                    console.debug('CNPJ preenchimento realizado com sucesso');
                } else {
                    clearEnderecoFields();
                    showCnpjError('CNPJ não encontrado na base de dados.');
                }
            },
            error: function(jqxhr, textStatus, error) {
                console.error('AJAX Error - Status:', jqxhr.status, 'Text:', textStatus, 'Error:', error);
                console.error('Response:', jqxhr.responseText);
                clearEnderecoFields();
                
                if (jqxhr.status === 404) {
                    showCnpjError('CNPJ não encontrado. Verifique o CNPJ informado.');
                } else if (jqxhr.status === 400) {
                    var errorMsg = 'CNPJ inválido. Verifique o número informado.';
                    try {
                        var errorResponse = JSON.parse(jqxhr.responseText);
                        if (errorResponse.erro) {
                            errorMsg = errorResponse.erro;
                        }
                    } catch (e) {
                        // Use default error message
                    }
                    showCnpjError(errorMsg);
                } else if (jqxhr.status === 500) {
                    showCnpjError('Erro ao validar CNPJ. Tente novamente mais tarde.');
                } else if (textStatus === 'timeout') {
                    showCnpjError('Tempo limite excedido. Tente novamente.');
                } else {
                    showCnpjError('Erro ao validar CNPJ. Tente novamente.');
                }
            }
        });
    }

    // Event listener for CNPJ input
    $cnpjInput.on('blur', function() {
        var cnpj = $(this).val();
        if (cnpj && cnpj.trim() !== '') {
            validateAndFillCnpj(cnpj);
        }
    });
});
