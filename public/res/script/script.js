// JsMate Chess Frontend - Modernized for 2025
// Works with the new ASP.NET Core Minimal API

const API_BASE_URL = 'http://localhost:5096/api';
const FADE_INTERVAL = 150;

// Chess piece Unicode characters
const PieceSymbols = {
    Rook:   { White: '\u2656', Black: '\u265C' },
    Knight: { White: '\u2658', Black: '\u265E' },
    Bishop: { White: '\u2657', Black: '\u265D' },
    Queen:  { White: '\u2655', Black: '\u265B' },
    King:   { White: '\u2654', Black: '\u265A' },
    Pawn:   { White: '\u2659', Black: '\u265F' }
};

// State
let selectedSquare = null;
let currentBoardId = null;
let validMoves = [];

// Utility Functions
function createGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0;
        const v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function getBoardIdFromCookie() {
    const match = document.cookie.match(/boardId=([^;]+)/);
    return match ? match[1] : null;
}

function setBoardIdCookie(boardId) {
    document.cookie = `boardId=${boardId}; expires=Fri, 31 Dec 9999 23:59:59 GMT; path=/`;
}

function clearBoardIdCookie() {
    document.cookie = 'boardId=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/';
}

// Status logging
function writeStatus(message) {
    const statusElement = document.getElementById('status');
    if (statusElement) {
        const timestamp = new Date().toLocaleTimeString();
        statusElement.innerHTML += `<br/>[${timestamp}] ${message}`;
        statusElement.scrollTop = statusElement.scrollHeight;
    }
    console.log(message);
}

function clearStatus() {
    const statusElement = document.getElementById('status');
    if (statusElement) {
        statusElement.innerHTML = '';
    }
}

// Board rendering
function clearBoard() {
    for (let row = 0; row < 8; row++) {
        for (let col = 0; col < 8; col++) {
            const square = document.querySelector(`.block.row${row}.col${col}`);
            if (square) {
                square.innerHTML = '';
                square.onclick = null;
            }
        }
    }
}

function clearBoardHighlights() {
    document.querySelectorAll('.block').forEach(square => {
        square.classList.remove('highlight', 'possibleMove', 'possibleAttack');
    });
}

function getPieceSymbol(pieceType, team) {
    const symbols = PieceSymbols[pieceType];
    if (!symbols) return '\u2639'; // Sad face for unknown
    return symbols[team] || symbols.White;
}

function renderBoard(boardData) {
    clearBoard();
    clearBoardHighlights();

    const pieces = boardData.Pieces || boardData.pieces || [];

    pieces.forEach(piece => {
        // Handle both old format and new API format (lowercase)
        const row = piece.row ?? piece.Row ?? piece.BoardPosition?.Row;
        const col = piece.col ?? piece.Col ?? piece.BoardPosition?.Col;
        const pieceType = piece.pieceType ?? piece.PieceType;
        const team = piece.pieceTeam ?? piece.PieceTeam;
        const isActive = piece.active ?? piece.Active ?? piece.IsActive ?? true;

        if (row === undefined || col === undefined) return;
        if (!isActive) return; // Skip captured pieces

        const square = document.querySelector(`.block.row${row}.col${col}`);
        if (!square) return;

        // Set piece symbol
        const symbol = getPieceSymbol(pieceType, team);
        square.innerHTML = symbol;

        // Set click handler for pieces
        square.onclick = (e) => {
            e.preventDefault();
            handleSquareClick(row, col, true);
        };
    });

    // Set click handlers for empty squares (for moves)
    for (let row = 0; row < 8; row++) {
        for (let col = 0; col < 8; col++) {
            const square = document.querySelector(`.block.row${row}.col${col}`);
            if (square && !square.innerHTML) {
                square.onclick = (e) => {
                    e.preventDefault();
                    handleSquareClick(row, col, false);
                };
            }
        }
    }

    // Update turn indicator if present
    const turnIndicator = document.getElementById('turnIndicator');
    if (turnIndicator) {
        const currentTurn = boardData.CurrentTurn || boardData.currentTurn || 'White';
        turnIndicator.textContent = `${currentTurn}'s turn`;
    }
}

// Click handling
async function handleSquareClick(row, col, hasPiece) {
    const clickedSquare = { row, col };

    // If we have a selected piece and clicked on a valid move
    if (selectedSquare && isValidMove(row, col)) {
        await executeMove(selectedSquare.row, selectedSquare.col, row, col);
        selectedSquare = null;
        validMoves = [];
        return;
    }

    // If clicking on a piece, select it and show moves
    if (hasPiece) {
        clearBoardHighlights();
        selectedSquare = clickedSquare;

        const square = document.querySelector(`.block.row${row}.col${col}`);
        if (square) {
            square.classList.add('highlight');
        }

        await showValidMoves(row, col);
    } else {
        // Clicking on empty square without valid move - deselect
        clearBoardHighlights();
        selectedSquare = null;
        validMoves = [];
    }
}

function isValidMove(row, col) {
    return validMoves.some(move => {
        const moveRow = move.row ?? move.Row;
        const moveCol = move.col ?? move.Col;
        return moveRow === row && moveCol === col;
    });
}

// API calls
async function fetchBoard(boardId) {
    try {
        writeStatus(`Fetching board: ${boardId}`);
        const response = await fetch(`${API_BASE_URL}/board/${boardId}`);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();
        writeStatus(`Board loaded successfully`);
        currentBoardId = data.Id || data.id || boardId;
        return data;
    } catch (error) {
        writeStatus(`Error fetching board: ${error.message}`);
        throw error;
    }
}

async function showValidMoves(row, col) {
    if (!currentBoardId) return;

    try {
        writeStatus(`Getting moves for piece at (${row}, ${col})`);
        const response = await fetch(`${API_BASE_URL}/board/${currentBoardId}/moves/${row}/${col}`);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const moves = await response.json();
        validMoves = moves;

        writeStatus(`Found ${moves.length} valid moves`);

        moves.forEach(move => {
            const targetRow = move.Row ?? move.row;
            const targetCol = move.Col ?? move.col;
            const square = document.querySelector(`.block.row${targetRow}.col${targetCol}`);

            if (square) {
                // Check if square has a piece (for attack highlighting)
                if (square.innerHTML) {
                    square.classList.add('possibleAttack');
                } else {
                    square.classList.add('possibleMove');
                }

                // Make valid move squares clickable
                const originalOnClick = square.onclick;
                square.onclick = (e) => {
                    e.preventDefault();
                    handleSquareClick(targetRow, targetCol, !!square.innerHTML);
                };
            }
        });
    } catch (error) {
        writeStatus(`Error getting moves: ${error.message}`);
        validMoves = [];
    }
}

async function executeMove(fromRow, fromCol, toRow, toCol) {
    if (!currentBoardId) return;

    try {
        writeStatus(`Moving piece from (${fromRow}, ${fromCol}) to (${toRow}, ${toCol})`);

        const response = await fetch(`${API_BASE_URL}/board/${currentBoardId}/move`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                fromRow: fromRow,
                fromCol: fromCol,
                toRow: toRow,
                toCol: toCol
            })
        });

        const result = await response.json();

        if (result.Success || result.success) {
            writeStatus('Move executed successfully');
            await refreshBoard();
        } else {
            writeStatus(`Move failed: ${result.Message || result.message || 'Unknown error'}`);
        }
    } catch (error) {
        writeStatus(`Error executing move: ${error.message}`);
    }
}

async function refreshBoard() {
    if (!currentBoardId) {
        currentBoardId = getBoardIdFromCookie();
    }

    if (!currentBoardId) {
        currentBoardId = createGuid();
        setBoardIdCookie(currentBoardId);
    }

    try {
        const boardData = await fetchBoard(currentBoardId);
        renderBoard(boardData);
    } catch (error) {
        writeStatus('Failed to load board');
    }
}

async function createNewBoard() {
    clearBoardIdCookie();
    currentBoardId = createGuid();
    setBoardIdCookie(currentBoardId);
    clearStatus();
    selectedSquare = null;
    validMoves = [];
    writeStatus('Creating new board...');
    await refreshBoard();
}

async function undoMove() {
    if (!currentBoardId) return;

    try {
        writeStatus('Undoing last move...');

        const response = await fetch(`${API_BASE_URL}/board/${currentBoardId}/undo`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const result = await response.json();

        if (result.Success || result.success) {
            writeStatus('Move undone successfully');
            clearBoardHighlights();
            selectedSquare = null;
            validMoves = [];
            await refreshBoard();
        } else {
            writeStatus(`Undo failed: ${result.Message || result.message || 'No moves to undo'}`);
        }
    } catch (error) {
        writeStatus(`Error undoing move: ${error.message}`);
    }
}

// Version display
async function setVersion() {
    try {
        const response = await fetch('version.json');
        const data = await response.json();
        const versionLabel = document.getElementById('versionLabel');
        if (versionLabel) {
            versionLabel.textContent = data.version;
        }
    } catch (error) {
        console.error('Failed to load version:', error);
    }
}

// Button handlers
function wireUpButtons() {
    const refreshBtn = document.getElementById('refreshboard');
    if (refreshBtn) {
        refreshBtn.onclick = () => {
            clearBoardHighlights();
            refreshBoard();
        };
    }

    const clearStatusBtn = document.getElementById('clearStatus');
    if (clearStatusBtn) {
        clearStatusBtn.onclick = clearStatus;
    }

    const newBoardBtn = document.getElementById('newBoard');
    if (newBoardBtn) {
        newBoardBtn.onclick = createNewBoard;
    }

    const undoBtn = document.getElementById('undoMove');
    if (undoBtn) {
        undoBtn.onclick = undoMove;
    }

    // Remove old move buttons that don't make sense with proper move logic
    const oldMoveButtons = ['moveBlackPawns', 'moveWhitePawns', 'showMoves'];
    oldMoveButtons.forEach(id => {
        const btn = document.getElementById(id);
        if (btn) {
            btn.style.display = 'none';
        }
    });
}

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    writeStatus('JsMate Chess - Initializing...');
    wireUpButtons();
    setVersion();

    currentBoardId = getBoardIdFromCookie();
    if (!currentBoardId) {
        currentBoardId = createGuid();
        setBoardIdCookie(currentBoardId);
    }

    refreshBoard();
});

// jQuery compatibility layer (for flash effects if jQuery is loaded)
if (typeof jQuery !== 'undefined') {
    jQuery.fn.extend({
        flashClick: function() {
            return this.fadeIn(FADE_INTERVAL).fadeOut(FADE_INTERVAL).fadeIn(FADE_INTERVAL);
        }
    });
}
