var localApiUrl = "http://localhost:44314/api/";
var prodApiUrl = "https://fedeleshandlesivcryptoprogram.azurewebsites.net/api/";

apiUrl = prodApiUrl;

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
