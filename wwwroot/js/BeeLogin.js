
var myVar;
var myVar2;
var myVar3;
var myVar4;

function myFunction() {
    myVar = setTimeout(showPage, 4000);
    myVar2 = setTimeout(fixPage, 4300);
    myVar3 = setTimeout(showHex, 4301);
    myVar4 = setTimeout(hexPulse, 6300);
}

function showPage() {
    document.getElementById("loader").classList.add("animate__animated");
    document.getElementById("loader").classList.add("animate__fadeOut");
}

function showHex() {
    document.getElementById("honeycomb").classList.add("animate__animated");
    document.getElementById("honeycomb").classList.add("animate__fadeIn");
    document.getElementById("honeycomb").classList.remove("d-none");
}

function hexPulse() {
    document.getElementById("hex").classList.add("animate__animated");
    document.getElementById("hex").classList.add("animate__pulse");
}

function fixPage() {
    document.getElementById("loader").classList.add("d-none");
}

window.onload = myFunction