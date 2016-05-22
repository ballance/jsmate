function setVersion() {
	try	{
		var ver = JSON.parse(readVersion('version.json')).version;
		var versionLabel = document.getElementById("versionLabel");
		versionLabel.innerHTML = ver;
	}
	catch (e)
	{

	}
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

function createGuid()
{
	var guid = 'zzzzzzzz-zzzz-4zzz-tzzz-zzzzzzzzzzzz'.replace(/[zt]/g, function(c) {
    	var r = Math.random()*16|0, v = c == 'z' ? r : (r&0x3|0x8);
    	return v.toString(16);
	});
	return guid;
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

function setBoardCookie() {
	if (document.cookie.replace(/(?:(?:^|.*;\s*)instanceCookie\s*\=\s*([^;]*).*$)|^.*$/, "$1") !== "true") {
    	document.cookie = "instanceCookie=true; expires=Fri, 31 Dec 9999 23:59:59 GMT";
		document.cookie = "boardId=" + createGuid() + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
	}
}

function clearBoardCookie() {
	document.cookie = "instanceCookie=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
	document.cookie = "boardId=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
}

function readBoardCookie() {
	var currentBardId = document.cookie.replace(/(?:(?:^|.*;\s*)boardId\s*\=\s*([^;]*).*$)|^.*$/, "$1");
	return currentBoardId;
}


function callApi() {
	var promise = $.getJSON('http://hipsterjesus.com/api/');

	promise.done(function(data) {
  		$('rolling-log').append(data.text);
	});

	promise.fail(function() {
  		$('rolling-log').append('<p>Oh no, something went wrong!</p>');
	});
}

function retrieveBoardState()
{
	// Get this jQuery madness out of here if time permits.  Plain Jane JS is more than sufficient
	var boardId = readBoardCookie();

	$.getJSON('http://localhost:9997/board/' + boardId)
		.done(function(data) {
			$('statuss').append(data.text);
		})
		.fail(function() {
			$('statuss').append('<p>API call failed');
		})
		.always(function() {
			$('statuss').append('<p>API call completed as promised</p>');			
		})
}

$( document ).ready(function() {
	setVersion();
	setBoardInitial();

	clearBoardCookie();
	setBoardCookie();

	retrieveBoardState();
	callApi();

});