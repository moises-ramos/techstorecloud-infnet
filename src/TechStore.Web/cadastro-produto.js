const API_BASE_URL = 'https://app-techstore-api-f2f7fhhjbge6bvgn.canadacentral-01.azurewebsites.net';
const API_PRODUTOS = `${API_BASE_URL}/api/produtos`;

let produtos = [];
let produtoParaExcluir = null;
let modoEdicao = false;

const formProduto = document.getElementById('formProduto');
const tabelaProdutos = document.getElementById('tabela-produtos');
const formTitle = document.getElementById('form-title');
const btnSubmitText = document.getElementById('btn-submit-text');
const btnSubmit = document.getElementById('btn-submit');
const btnCancelEdit = document.getElementById('btn-cancel-edit');
const searchInput = document.getElementById('search-input');
const emptyState = document.getElementById('empty-state');
const tableContainer = document.getElementById('tabela-produtos-container');

document.addEventListener('DOMContentLoaded', () => {
    carregarProdutos();
});

async function carregarProdutos() {
    try {
        const resposta = await fetch(API_PRODUTOS);

        if (!resposta.ok) {
            throw new Error(`Erro HTTP: ${resposta.status}`);
        }

        produtos = await resposta.json();
        renderizarTabela(produtos);
        atualizarEstatisticas(produtos);

    } catch (erro) {
        console.error('Erro ao carregar produtos:', erro);
        tabelaProdutos.innerHTML = `
            <tr>
                <td colspan="6" class="loading-cell">
                    <span style="color: var(--rose-400);">Erro ao conectar com a API</span>
                    <br><small style="color: var(--text-muted);">${erro.message}</small>
                </td>
            </tr>
        `;
        showToast('Erro ao carregar produtos. Verifique se a API está online.', 'error');
    }
}

formProduto.addEventListener('submit', async function (event) {
    event.preventDefault();

    const produtoId = document.getElementById('produto-id').value;

    const produto = {
        nome: document.getElementById('nome').value.trim(),
        descricao: document.getElementById('descricao').value.trim(),
        categoria: document.getElementById('categoria').value,
        preco: parseFloat(document.getElementById('preco').value),
        quantidadeEstoque: parseInt(document.getElementById('estoque').value)
    };

    try {
        let resposta;
        if (modoEdicao && produtoId) {
            resposta = await fetch(`${API_PRODUTOS}/${produtoId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(produto)
            });
        } else {            
            resposta = await fetch(API_PRODUTOS, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(produto)
            });
        }

        if (!resposta.ok) {
            const errorData = await resposta.json().catch(() => null);
            throw new Error(errorData?.message || `Erro HTTP: ${resposta.status}`);
        }

        formProduto.reset();
        document.getElementById('produto-id').value = '';

        if (modoEdicao) {
            cancelarEdicao();
            showToast('Produto atualizado com sucesso!', 'success');
        } else {
            showToast('Produto cadastrado com sucesso!', 'success');
        }

        await carregarProdutos();

    } catch (erro) {
        console.error('Erro ao salvar produto:', erro);
        showToast('Erro ao salvar produto. Tente novamente.', 'error');
    }
});

function editarProduto(id) {
    const produto = produtos.find(p => p.id === id);

    if (!produto) return;

    document.getElementById('produto-id').value = produto.id;
    document.getElementById('nome').value = produto.nome;
    document.getElementById('descricao').value = produto.descricao || '';
    document.getElementById('categoria').value = produto.categoria;
    document.getElementById('preco').value = produto.preco;
    document.getElementById('estoque').value = produto.quantidadeEstoque;

    modoEdicao = true;
    formTitle.innerHTML = `
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7"/>
            <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z"/>
        </svg>
        Editar Produto #${id}
    `;
    btnSubmitText.textContent = 'Atualizar Produto';
    btnSubmit.classList.add('btn-update');
    btnCancelEdit.style.display = 'block';
    
    document.getElementById('form-section').scrollIntoView({ behavior: 'smooth', block: 'start' });
    document.getElementById('nome').focus();
}

function cancelarEdicao() {
    modoEdicao = false;
    document.getElementById('produto-id').value = '';
    formProduto.reset();

    formTitle.innerHTML = `
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M12 4v16m8-8H4"/>
        </svg>
        Cadastrar Produto
    `;
    btnSubmitText.textContent = 'Cadastrar Produto';
    btnSubmit.classList.remove('btn-update');
    btnCancelEdit.style.display = 'none';
}

function excluirProduto(id) {
    const produto = produtos.find(p => p.id === id);

    if (!produto) return;

    produtoParaExcluir = id;
    document.getElementById('modal-message').textContent =
        `Tem certeza que deseja excluir o produto "${produto.nome}"?`;

    document.getElementById('modal-overlay').classList.add('show');
}

async function confirmarExclusao() {
    if (!produtoParaExcluir) return;

    try {
        const resposta = await fetch(`${API_PRODUTOS}/${produtoParaExcluir}`, {
            method: 'DELETE'
        });

        if (!resposta.ok) {
            throw new Error(`Erro HTTP: ${resposta.status}`);
        }

        fecharModal();
        showToast('Produto excluído com sucesso!', 'success');
        await carregarProdutos();

    } catch (erro) {
        console.error('Erro ao excluir produto:', erro);
        fecharModal();
        showToast('Erro ao excluir produto. Tente novamente.', 'error');
    }
}

function fecharModal() {
    document.getElementById('modal-overlay').classList.remove('show');
    produtoParaExcluir = null;
}

function renderizarTabela(listaProdutos) {
    if (listaProdutos.length === 0) {
        tabelaProdutos.innerHTML = '';
        tableContainer.style.display = 'none';
        emptyState.style.display = 'flex';
        return;
    }

    tableContainer.style.display = '';
    emptyState.style.display = 'none';

    tabelaProdutos.innerHTML = listaProdutos.map(produto => `
        <tr>
            <td style="color: var(--text-muted); font-variant-numeric: tabular-nums;">#${produto.id}</td>
            <td>
                <div class="product-name">${escapeHtml(produto.nome)}</div>
                ${produto.descricao ? `<div class="product-desc" title="${escapeHtml(produto.descricao)}">${escapeHtml(produto.descricao)}</div>` : ''}
            </td>
            <td><span class="category-badge">${escapeHtml(produto.categoria)}</span></td>
            <td class="price-cell">R$ ${Number(produto.preco).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
            <td class="${getStockClass(produto.quantidadeEstoque)} stock-cell">${produto.quantidadeEstoque}</td>
            <td>
                <div class="actions-cell">
                    <button class="btn-action btn-edit" onclick="editarProduto(${produto.id})" title="Editar produto">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7"/>
                            <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z"/>
                        </svg>
                    </button>
                    <button class="btn-action btn-delete" onclick="excluirProduto(${produto.id})" title="Excluir produto">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M3 6h18M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6m3 0V4a2 2 0 012-2h4a2 2 0 012 2v2"/>
                            <line x1="10" y1="11" x2="10" y2="17"/>
                            <line x1="14" y1="11" x2="14" y2="17"/>
                        </svg>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

function filtrarProdutos() {
    const termo = searchInput.value.toLowerCase().trim();

    if (!termo) {
        renderizarTabela(produtos);
        return;
    }

    const filtrados = produtos.filter(p =>
        p.nome.toLowerCase().includes(termo) ||
        p.categoria.toLowerCase().includes(termo) ||
        (p.descricao && p.descricao.toLowerCase().includes(termo))
    );

    renderizarTabela(filtrados);
}

function atualizarEstatisticas(listaProdutos) {
    document.getElementById('stat-total').textContent = listaProdutos.length;

    const categorias = [...new Set(listaProdutos.map(p => p.categoria))];
    document.getElementById('stat-categories').textContent = categorias.length;
    
    const valorTotal = listaProdutos.reduce((acc, p) => acc + (p.preco * p.quantidadeEstoque), 0);
    document.getElementById('stat-value').textContent =
        `R$ ${valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 0, maximumFractionDigits: 0 })}`;

    const totalEstoque = listaProdutos.reduce((acc, p) => acc + p.quantidadeEstoque, 0);
    document.getElementById('stat-stock').textContent = totalEstoque.toLocaleString('pt-BR');
}

function getStockClass(quantidade) {
    if (quantidade <= 10) return 'stock-low';
    if (quantidade <= 50) return 'stock-medium';
    return 'stock-high';
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function showToast(message, type = 'info') {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.className = `toast toast-${type} show`;

    setTimeout(() => {
        toast.classList.remove('show');
    }, 4000);
}
