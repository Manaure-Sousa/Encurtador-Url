const API_URL = 'http://localhost:5066';

const urlInput = document.getElementById('urlInput');
const shortenBtn = document.getElementById('shortenBtn');
const resultDiv = document.getElementById('result');
const shortUrlInput = document.getElementById('shortUrl');
const copyBtn = document.getElementById('copyBtn');
const loadingDiv = document.getElementById('loading');
const errorDiv = document.getElementById('error');
const errorMessage = document.getElementById('errorMessage');

let urlGerada = null;

shortenBtn.addEventListener('click', async () => {
    if (urlGerada) {
        // Se já tem resultado, limpa tudo
        urlInput.value = '';
        resultDiv.style.display = 'none';
        loadingDiv.style.display = 'none';
        errorDiv.style.display = 'none';
        shortenBtn.textContent = 'Encurtar';
        shortenBtn.style.background = 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)';
        urlGerada = null;
        urlInput.focus();
        return;
    }

    const url = urlInput.value.trim();

    if (!url) {
        mostrarErro('Por favor, insira uma URL');
        return;
    }

    if (!validarUrl(url)) {
        mostrarErro('URL inválida. Insira uma URL começando com http:// ou https://');
        return;
    }

    resultDiv.style.display = 'none';
    errorDiv.style.display = 'none';
    loadingDiv.style.display = 'flex';
    shortenBtn.disabled = true;

    try {
        const response = await fetch(`${API_URL}/shorten`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ url })
        });

        if (!response.ok) throw new Error('Erro ao encurtar URL');

        const shortUrl = await response.json();
        
        // MOSTRAR RESULTADO - COM DISPLAY DIRETO
        loadingDiv.style.display = 'none';
        resultDiv.style.display = 'block';
        shortUrlInput.value = shortUrl;
        shortUrlInput.select();
        
        // SALVAR QUE TEM RESULTADO
        urlGerada = shortUrl;
        
        // MUDAR BOTÃO
        shortenBtn.textContent = 'Gerar outra URL';
        shortenBtn.style.background = 'linear-gradient(135deg, #4caf50 0%, #45a049 100%)';

    } catch (error) {
        console.error('Erro:', error);
        mostrarErro('Erro ao conectar com a API. Verifique se o servidor está rodando.');
    } finally {
        shortenBtn.disabled = false;
    }
});

copyBtn.addEventListener('click', () => {
    navigator.clipboard.writeText(shortUrlInput.value).then(() => {
        const originalText = copyBtn.querySelector('.copy-text').textContent;
        copyBtn.querySelector('.copy-text').textContent = 'Copiado!';
        copyBtn.style.background = '#4caf50';
        copyBtn.style.color = 'white';
        
        setTimeout(() => {
            copyBtn.querySelector('.copy-text').textContent = originalText;
            copyBtn.style.background = '';
            copyBtn.style.color = '';
        }, 1500);
    });
});

urlInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter' && !urlGerada) {
        shortenBtn.click();
    }
});

function validarUrl(string) {
    try {
        new URL(string);
        return true;
    } catch (_) {
        return false;
    }
}

function mostrarErro(msg) {
    resultDiv.style.display = 'none';
    loadingDiv.style.display = 'none';
    errorDiv.style.display = 'flex';
    errorMessage.textContent = msg;
}

document.addEventListener('DOMContentLoaded', () => {
    urlInput.focus();
    resultDiv.style.display = 'none';
    loadingDiv.style.display = 'none';
    errorDiv.style.display = 'none';
});
