const FavoritoService = (() => {
    const STORAGE_KEY = "zf_favoritos";
    const rota = "/api/usuarios/me/favoritos";

    function listarLocal() {
        try {
            const dados = JSON.parse(localStorage.getItem(STORAGE_KEY));
            return Array.isArray(dados) ? dados : [];
        } catch {
            return [];
        }
    }

    async function listarIds() {
        return ZeroFrameApi.normalizarLista(await ZeroFrameApi.request(rota)).map(String);
    }

    async function listar() {
        const ids = await listarIds();
        const produtos = await Promise.all(ids.map(async (id) => {
            try {
                return await ProdutoService.buscarPorId(id);
            } catch (erro) {
                if (erro.status === 404) return null;
                throw erro;
            }
        }));
        return produtos.filter(Boolean);
    }

    async function alternar(produtoId) {
        const id = String(produtoId);
        const jaSalvo = (await listarIds()).includes(id);
        await ZeroFrameApi.request(`${rota}/${encodeURIComponent(id)}`, {
            method: jaSalvo ? "DELETE" : "PUT"
        });
        return !jaSalvo;
    }

    async function importarDoNavegador() {
        const ids = new Set(listarLocal().map((produto) => Number(ProdutoService.getProdutoId(produto)))
            .filter((id) => Number.isInteger(id) && id > 0));
        for (const id of ids) {
            await ZeroFrameApi.request(`${rota}/${id}`, { method: "PUT" });
        }
        localStorage.removeItem(STORAGE_KEY);
    }

    return { listar, listarIds, alternar, importarDoNavegador, temFavoritosLocais: () => listarLocal().length > 0 };
})();
