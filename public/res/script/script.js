const fadeInterval = 150;

function setVersion() {
	try	{
		var ver = JSON.parse(readVersion('version.json')).version;
		var versionLabel = document.getElementById("versionLabel");
		versionLabel.innerHTML = ver;
	}
	catch (e)
	{
		writeStatus(e);
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

// Extend jQuery to provide flash UX on pieces
jQuery.fn.extend({
  flashClick: function() {
    return this.fadeIn(fadeInterval).fadeOut(fadeInterval).fadeIn(fadeInterval);
    }});
  

 const RookHtml = { White:'&#9814;', Black:'&#9820;'};
 const KnightHtml = { White:'&#9816;', Black:'&#9821;'};
 const BishopHtml = { White:'&#9815', Black:'&#9822;'};
 const QueenHtml = { White:'&#9813', Black:'&#9819;'};
 const KingHtml = { White:'&#9812;', Black:'&#9818;'};
 const PawnHtml = { White:'&#9817;', Black:'&#9823;'};
 
function setBoardInitial()
{
	/*
	// King
	var blackKing = $(".block.row1.col5");
	blackKing.html('&#9818;');
	blackKing.click(function() { writeStatus('clicked black king'); blackKing.flashClick();});
	
	// Queen
	var blackQueen = $(".block.row1.col4");
	blackQueen.html('&#9819;');
	blackQueen.click(function() { writeStatus('clicked black queen'); blackQueen.flashClick();});
	
	// Pawns
	$(".block.row2.block").html('&#9823;');
	$(".block.row2.block").click(function() { 
		writeStatus('clicked a black pawn');
	});
	*/
}

function setBoardCookie() {
	if (document.cookie.replace(/(?:(?:^|.*;\s*)instanceCookie\s*\=\s*([^;]*).*$)|^.*$/, "$1") !== "true") {
    	var newGuid = createGuid();
    	document.cookie = "instanceCookie=true; expires=Fri, 31 Dec 9999 23:59:59 GMT";
		document.cookie = "boardId=" + newGuid + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
	}
}

function clearBoardCookie() {
	document.cookie = "instanceCookie=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
	document.cookie = "boardId=; expires=Fri, 31 Dec 9999 23:59:59 GMT";
}

function readBoardIdFromCookie() {
	var currentBoardId = document.cookie.replace(/(?:(?:^|.*;\s*)boardId\s*\=\s*([^;]*).*$)|^.*$/, "$1");
	return currentBoardId;
}

function clearBoard() {
	for (var i = 0; i < 8; i++) {
		for (var j = 0; j < 8; j++)	{
			$('.block.row' + i + '.col' + j).html('');
		}
	}
}

function loadBoardFromReceivedData(data)
{
	try
	{
		clearBoard();

		var pieceArray = data.Pieces;
		writeStatus('Retrieved [' + pieceArray.length + '] pieces.')
		for (var i = 0; i < pieceArray.length; i++) {
			var piece = pieceArray[i];

			var pieceHtml = '&#9785;';  // default sad face
			switch(piece.PieceType) 
			{
				case "Queen":
					pieceHtml = QueenHtml;
					break;
				case "King":
					pieceHtml = KingHtml;
					break;
				case "Bishop":
					pieceHtml = BishopHtml;
					break;
				case "Knight":
					pieceHtml = KnightHtml;
					break;
				case "Rook":
					pieceHtml = RookHtml;
					break;
				case "Pawn":
					pieceHtml = PawnHtml;
					break;
			}

			if (piece.PieceTeam == '0') {
				pieceHtml = pieceHtml.Black;
			}
			else {
				pieceHtml = pieceHtml.White;
			}


			$('.block.row' + piece.BoardPosition.Row + '.col' + piece.BoardPosition.Col).html(pieceHtml);
			//writeStatus(piece.PieceType + ' / pieceNumber ' + piece.PieceNumber  + ' / color ' + piece.PieceTeam + ' / col ' + piece.BoardPosition.Col + ' / row ' + piece.BoardPosition.Row);
		}
	}	
	catch (e)
	{
		writeStatus(e);
	}
	//setBoardInitial();
}


function retrieveBoardState()
{
	// Get this jQuery madness out of here if time permits.  Plain Jane JS is more than sufficient
	var boardId = readBoardIdFromCookie();
	writeStatus('Attempting to get board: [' + boardId + ']');
	
	var apiUri = 'http://localhost:9997/board/' + boardId;
	
	$.getJSON(apiUri)
		.done(function(data) {
			try
			{
				writeStatus('Success! Retrieved board: [' + data.Id + ']');

				loadBoardFromReceivedData(data);
			}
			catch(e)
			{
				writeStatus('Failed to deserialize json board')
			}
		})
		.fail(function() {
			writeStatus('<p>API call to ' + apiUri + ' failed</p>');

		})
		.always(function() {
			//writeStatus('<p>API call completed as promised</p>');	
			//writeStatus('APIprom ise completed');		
		})
}

function clearStatus()
{
	$('#statuss').html('');
}

function writeStatus(statusMessage)
{
	$('#statuss').append('<br />' + statusMessage);
}



function wireUpButtons()
{
	$('#refreshboard').click(function() {
		retrieveBoardState();
	});


	$('#clearStatus').click(function() {
		clearStatus();
	});

	$('#newBoard').click(function() {
		clearBoardCookie();
		clearStatus();
		setBoardCookie();
		retrieveBoardState();
	});

	$('#movePawn').click(function() {

		var boardId = readBoardIdFromCookie();
	
		var apiUri = 'http://localhost:9997/move/' + boardId + '/0/Pawn/1';
		writeStatus('moving piece: ' + apiUri);

		$.getJSON(apiUri)
			.done(function(data) {
				try
				{
					writeStatus('moved board: ' + data);
				}
				catch(e)
				{
					writeStatus('failed to deserialize json move')
				}
			})
			.fail(function() {
				writeStatus('<p>API call to ' + apiUri + ' failed</p>');

			})
			.always(function() {
				retrieveBoardState();
			})

	});


}

$(document).ready(function() {
	wireUpButtons();

	setVersion();
	//clearBoardCookie();
	setBoardCookie();

	retrieveBoardState();

	//setBoardInitial();


});