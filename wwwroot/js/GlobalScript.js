function GenerateQRCode(id, data) {
    new QRCode(document.getElementById(id), data)
}