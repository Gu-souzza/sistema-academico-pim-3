document.addEventListener("DOMContentLoaded", async () => {
    if (!ZeroFrameApi.protegerPagina()) return;

    const lista = document.querySelector(".favorites-list");
    if (!lista) return;

    if (FavoritoService.temFavoritosLocais()) {
        const importar = document.createElement("button");
        importar.type = "button";
        importar.textContent = "Importar favoritos deste navegador";
        importar.addEventListener("click", async () => {
            if (!window.confirm("Importar os favoritos deste navegador para a conta aberta?")) return;
            importar.disabled = true;
            try {
                await FavoritoService.importarDoNavegador();
                window.location.reload();
            } catch (erro) {
                importar.disabled = false;
                ZeroFrameApi.notificar(ZeroFrameApi.tratarErro(erro, "Não foi possível importar os favoritos."));
            }
        });
        lista.before(importar);
    }

    try {
        ZeroFrameApi.mostrarCarregando(lista, "Carregando favoritos...");
        const favoritos = await FavoritoService.listar();

        if (!favoritos.length) {
            ZeroFrameApi.mostrarMensagem(lista, "Você ainda não tem produtos favoritos.");
            return;
        }

        lista.textContent = "";
        favoritos.forEach((item) => {
            const produto = item.produto || item.Produto || item;
            lista.appendChild(ProdutoService.criarCard(produto, {
                productPath: "../../pages/produtos/product.html",
                fallbackImage: "../../assets/products/camisa-over-black.png",
                tagName: "article"
            }));
        });
    } catch (error) {
        ZeroFrameApi.mostrarMensagem(lista, ZeroFrameApi.tratarErro(error, "Erro ao carregar favoritos."));
    }
});
