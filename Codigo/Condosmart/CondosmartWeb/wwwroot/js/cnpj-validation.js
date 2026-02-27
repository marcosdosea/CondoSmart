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
        // Remove non-digits
        var cleanCnpj = cnpj.replace(/\D/g, '');
        
        // Validate length
        if (cleanCnpj.length !== 14) {
            return false;
        }
        
        // Check if all digits are the same (invalid CNPJ)
        if (/^(\d)\1{13}$/.test(cleanCnpj)) {
            return false;
        }
        
        // Calculate first verifier digit
        var size = cleanCnpj.length - 2;
        var numbers = cleanCnpj.substring(0, size);
        var digits = cleanCnpj.substring(size);
        var sum = 0;
        var pos = size - 7;

        for (var i = size; i >= 1; i--) {
            sum += numbers.charAt(size - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        var result = sum % 11 < 2 ? 0 : 11 - sum % 11;
        if (result != digits.charAt(0))
            return false;

        // Calculate second verifier digit
        size = size + 1;
        numbers = cleanCnpj.substring(0, size);
        sum = 0;
        pos = size - 7;

        for (var i = size; i >= 1; i--) {
            sum += numbers.charAt(size - i) * pos--;
            if (pos < 2)
                pos = 9;
        }

        result = sum % 11 < 2 ? 0 : 11 - sum % 11;
        if (result != digits.charAt(1))
            return false;

        return true;
    }

    function validateAndFillCnpj(cnpj) {
        // Remove non-digits
        var cleanCnpj = cnpj.replace(/\D/g, '');
        
        // Validate format first
        if (!validateCnpjFormat(cleanCnpj)) {
            clearEnderecoFields();
            showCnpjError('CNPJ inválido. Verifique o número informado.');
            return;
        }

        console.debug('Validating CNPJ:', cleanCnpj);

        // Fetch from API
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
                } else {
                    clearEnderecoFields();
                    showCnpjError('CNPJ não encontrado ou inválido.');
                }
            },
            error: function(jqxhr, textStatus, error) {
                console.error('AJAX Error - Status:', jqxhr.status, 'Text:', textStatus, 'Error:', error);
                console.error('Response:', jqxhr.responseText);
                clearEnderecoFields();
                
                if (jqxhr.status === 404) {
                    showCnpjError('CNPJ não encontrado. Verifique o CNPJ informado.');
                } else if (jqxhr.status === 400) {
                    showCnpjError('CNPJ inválido. Verifique o número informado.');
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
