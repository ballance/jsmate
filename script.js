function setVersion() {
	var ver = JSON.parse(readVersion('version.json')).version;
	var versionLabel = document.getElementById("versionLabel");
	versionLabel.innerHTML = ver;
}

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

function setBoardInitial()
{
	// White
	
	// Rooks
	$(".block.row8.col1").html('&#9814');
	$(".block.row8.col8").html('&#9814');

	// Bishops
	$(".block.row8.col2").html('&#9816;');
	$(".block.row8.col7").html('&#9816;');

	// Knights
	$(".block.row8.col3").html('&#9815;');
	$(".block.row8.col6").html('&#9815;');

	// King
	$(".block.row8.col4").html('&#9813;');
	
	// Queen
	$(".block.row8.col5").html('&#9812;');

	// Pawns
	$(".block.row7.block").html('&#9817;');

	// Black

	// Rooks
	$(".block.row1.col1").html('&#9820');
	$(".block.row1.col8").html('&#9820');

	// Bishops
	$(".block.row1.col2").html('&#9822;');
	$(".block.row1.col7").html('&#9822;');

	// Knights
	$(".block.row1.col3").html('&#9821;');
	$(".block.row1.col6").html('&#9821;');

	// King
	$(".block.row1.col5").html('&#9818;');
	
	// Queen
	$(".block.row1.col4").html('&#9819;');

	// Pawns
	$(".block.row2.block").html('&#9823;');
}


$( document ).ready(function() {
	setVersion();
	setBoardInitial();
});