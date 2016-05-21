
var ready = function ( fkn ) {
    if ( typeof fkn !== 'function' ) return;

    if ( document.readyState === 'complete'  ) {
        return fkn();
    }

    document.addEventListener( 'interactive', fkn, false );
};

ready(function() {


    var article = document.getElementById("article");
   
    alert(article.value);
    alert('yo');

});


function readVersion(file)
{
	var versionRawFound = '';
    var versionRaw = new XMLHttpRequest();
    versionRaw.open("GET", file, false);
    versionRaw.onreadystatechange = function ()
    {
        if(versionRaw.readyState === 4)
        {
            if(versionRaw.status === 200 || versionRaw.status == 0)
            {
                versionRawFound = versionRaw.responseText;
            }
        }
    }
    versionRaw.send(null);

    return versionRawFound;
}

var ver = JSON.parse(readVersion('version.json')).version;
console.log('version:' + ver);
var versionLabel = document.getElementById("versionLabel");
versionLabel.innerHTML = ver;
