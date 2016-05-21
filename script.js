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
	$(".block.row8.col1").click(function() { console.log('clicked white rook 1');});
	$(".block.row8.col8").html('&#9814');
	$(".block.row8.col8").click(function() { console.log('clicked white rook 2');});

	// Knights
	$(".block.row8.col2").html('&#9816;');
	$(".block.row8.col2").click(function() { console.log('clicked white knight 1');});
	$(".block.row8.col7").html('&#9816;');
	$(".block.row8.col7").click(function() { console.log('clicked white knight 2');});


	// Knights
	$(".block.row8.col3").html('&#9815;');
	$(".block.row8.col3").click(function() { console.log('clicked white bishop 1');});
	$(".block.row8.col6").html('&#9815;');
	$(".block.row8.col6").click(function() { console.log('clicked white bishop 2');});

	// Queen
	$(".block.row8.col4").html('&#9813;');
	$(".block.row8.col4").click(function() { console.log('clicked the white queen');});

	// King
	$(".block.row8.col5").html('&#9812;');
	$(".block.row8.col5").click(function() { console.log('clicked the white king');});
	
	// Pawns
	$(".block.row7.block").html('&#9817;');
	$(".block.row7.block").click(function() { 
		console.log('clicked a white pawn');
	});


	// Black

	// Rooks
	$(".block.row1.col1").html('&#9820');
	$(".block.row1.col1").click(function() { console.log('clicked black rook 1');});
	$(".block.row1.col8").html('&#9820');
	$(".block.row1.col8").click(function() { console.log('clicked black rook 2');});
	
	// Bishops
	$(".block.row1.col2").html('&#9822;');
	$(".block.row1.col2").click(function() { console.log('clicked black knight 1');});
	$(".block.row1.col7").html('&#9822;');
	$(".block.row1.col7").click(function() { console.log('clicked black knight 2');});

	// Knights
	$(".block.row1.col3").html('&#9821;');
	$(".block.row1.col3").click(function() { console.log('clicked black bishop 1');});
	$(".block.row1.col6").html('&#9821;');
	$(".block.row1.col6").click(function() { console.log('clicked black bishop 2');});

	// King
	$(".block.row1.col5").html('&#9818;');
	$(".block.row1.col5").click(function() { console.log('clicked black king');});
	
	// Queen
	$(".block.row1.col4").html('&#9819;');
	$(".block.row1.col4").click(function() { console.log('clicked black queen');});
	
	// Pawns
	$(".block.row2.block").html('&#9823;');
	$(".block.row2.block").click(function() { 
		console.log('clicked a black pawn');
	});
}


$( document ).ready(function() {
	setVersion();
	setBoardInitial();
});