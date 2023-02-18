let list = document.getElementById('titles-list');
let titlesInput = document.getElementById('search-article-text');

titlesInput.addEventListener('keyup', autocomplete);

function autocomplete(event) {
    let value = event.target.value;
    list.innerHTML = '';

    if (value.length >= 1) {
        fetch(`/article/gettitles?title=${value}`, { method: 'get' })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                else {
                    console.log(response.status);
                    return null;
                }
            })
            .then((json) => {
                console.log(json);
                json.forEach(function (title) {
                    let opt = document.createElement('option');
                    opt.value = title;
                    list.appendChild(opt);
                });
            });
    }
}