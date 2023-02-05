let searchBtn = document.getElementById('search-article-btn');

searchBtn.addEventListener("click", searchArticles);

function searchArticles() {
    let searchTxt = document.getElementById('search-article-text').value;

    console.log(searchTxt);

    let articlesList = document.getElementById('article-list');
    articlesList.innerHTML = '';

    let spinner = document.getElementById('spinner');
    spinner.removeAttribute('hidden');

    let data = {
        searchText: searchTxt
    };

    fetch('/article/search', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then((response) => {
            if (response.ok) {
                /*return response.json();*/
                return response.text();
            }
            else {
                console.log(response.status);
                return null;
            }
        })
        .then((html) => {
            articlesList.innerHTML = html;
        });

}
        //.then((json) => {
        //    for (var i = 0; i < json.length; i++) {
        //        var item = json[i];
        //    }

        //    return;
        //});