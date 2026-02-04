document.getElementById("checkAll").onclick = function () {
    var checkboxes = document.getElementsByClassName('checkbox-item');
    for (var checkbox of checkboxes) {
        checkbox.checked = this.checked;
    }
}
function ExportarExcel() {
    const selecionados = Array.from(document.querySelectorAll('.checkbox-item:checked'))
        .map(cb => parseInt(cb.value));

    if (selecionados.length === 0) {
        alert("Por favor, selecione pelo menos um contato para exportar.");
        return;
    }

    fetch('/Contato/ExportarExcel', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(selecionados)
    })
        .then(response => {
            if (response.ok) return response.blob();
            throw new Error('Resposta da rede não OK');
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = "Contatos_Selecionados.xlsx";
            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(err => alert(err.message));
}
function Editar(id) {
    if (!id) {
        console.warn('Editar: id não fornecido');
        return;
    }

    fetch(`/Contato/Editar/${id}`)
        .then(response => {
            if (!response.ok) throw new Error('Resposta da rede não OK');
            return response.json();
        })
        .then(data => {
            document.getElementById("Nome").value = data.nome || "";
            document.getElementById("Email").value = data.email || "";
            document.getElementById("Telefone").value = data.telefone || "";
            document.getElementById("Status").value = data.status ?? "";
            document.getElementById("Id").value = data.id ?? id;

            const modalEl = document.getElementById('EditarModal');
            if (modalEl) {
                const modal = new bootstrap.Modal(modalEl);
                modal.show();
            }
        })
        .catch(err => console.error('Erro ao carregar contato:', err));
}
function Deletar(id) {
        fetch(`/Contato/Deletar/${id}`, {
            method: 'DELETE' 
        })
        .then(response => {
            if (!response.ok) throw new Error('Resposta da rede não OK');
            return response.json();

        })
        .then(data => {
            document.getElementById("Id").value = data.id ?? id;
        })
        .then(data => {
            if (data && data.success) {
                alert('Erro ao salvar alterações.');
                console.error('Deletar: resposta do servidor:', data);
            } else {
                alert('Sucesso.');
                location.reload();
            
        }
        })
        .catch(err => console.error('Erro ao carregar contato:', err));
}
function SalvarEdit() {
    const form = document.getElementById('formEditar');

    fetch('/Contato/EditarPessoa', {
        method: 'POST',
        body: new FormData(form)
    })
    .then(response => {
        if (!response.ok) throw new Error('Resposta da rede não OK: ' + response.status);
        return response.json();
    })
    .then(data => {
        if (data || data.success) {
            location.reload();
        } else {
            alert('Erro ao salvar alterações.');
            console.error('SalvarEdit: resposta do servidor:', data);
        }
    })
    .catch(err => {
        console.error('Erro ao salvar contato:', err);
        alert('Ocorreu um erro ao salvar. Verifique o console para mais detalhes.');
    });
}
function Criar() {
            document.getElementById("Id").value = 0;
            document.getElementById("Nome").value = "";
            document.getElementById("Email").value = "";
            document.getElementById("Telefone").value = "";
            document.getElementById("Status").value = "";

            var modalEl = document.getElementById('CriarModal');
            var Modal = new bootstrap.Modal(modalEl);
            Modal.show();
}
function CriarUsuario() {
    const form = document.getElementById('formCriar');

    if (!form) return console.error("Formulário não encontrado!");

    fetch('/Contato/Criar', {
        method: 'POST',
        body: new FormData(form)
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                window.location.reload();
            } else {
                alert('Erro ao criar usuário: ' + (data.message || 'Verifique os dados.'));
                window.location.reload();
            }
        })
        .catch(err => {
            console.error('Erro na requisição:', err);
            alert('Erro crítico ao conectar com o servidor.');
        });
}
