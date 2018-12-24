
var apiUrl = "http://localhost:44314/api/";

function encodeText() {
    var text = document.querySelector("#encryptText").value;
    console.log(text);
    var request = {
        method: "GET"
    };

    var url = apiUrl + "Steganography/GetEncode?text=" + text;
    fetch(url);
}


function getText() {
    var request = {
        method: "GET"
    };

    var url = apiUrl + "Steganography/SecretText";

    fetch(url, request)
        .then(response => response.json())
        .then(data => document.querySelector("#decryptText").value = data.text);
}
