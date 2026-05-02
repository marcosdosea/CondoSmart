(function () {
    function findField(form, name) {
        return form.querySelector(`[name='${name}']`);
    }

    function fillAddress(form, data) {
        const rua = findField(form, 'Rua');
        const bairro = findField(form, 'Bairro');
        const complemento = findField(form, 'Complemento');
        const cidade = findField(form, 'Cidade');
        const uf = findField(form, 'Uf');
        const cep = findField(form, 'Cep');

        if (rua) rua.value = data.rua || '';
        if (bairro) bairro.value = data.bairro || '';
        if (complemento && !complemento.value) complemento.value = data.complemento || '';
        if (cidade) cidade.value = data.cidade || '';
        if (uf) uf.value = data.uf || '';
        if (cep) cep.value = data.cep || '';
    }

    function clearAlert(form) {
        const oldAlert = form.querySelector('.cep-error-alert');
        if (oldAlert) oldAlert.remove();
    }

    function showAlert(form, input, message) {
        clearAlert(form);
        const alert = document.createElement('div');
        alert.className = 'alert alert-danger mt-2 cep-error-alert';
        alert.textContent = message;
        input.parentElement.appendChild(alert);
    }

    async function handleCepBlur(event) {
        const input = event.target;
        const form = input.form;
        if (!form) return;

        const digits = (input.value || '').replace(/\D/g, '');
        clearAlert(form);

        if (!digits) return;
        if (digits.length !== 8) {
            showAlert(form, input, 'CEP deve conter exatamente 8 digitos.');
            return;
        }

        try {
            const response = await fetch(`/Cep/GetEndereco?cep=${encodeURIComponent(digits)}`);
            if (!response.ok) {
                if (response.status === 404) {
                    showAlert(form, input, 'CEP nao encontrado.');
                    return;
                }

                showAlert(form, input, 'Nao foi possivel validar o CEP agora.');
                return;
            }

            const data = await response.json();
            fillAddress(form, data);
        } catch {
            showAlert(form, input, 'Erro ao consultar o CEP.');
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll("input[name='Cep']").forEach(function (input) {
            input.addEventListener('blur', handleCepBlur);
        });
    });
})();
