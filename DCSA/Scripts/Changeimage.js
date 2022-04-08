

let img = document.querySelector(".Header-Homepage");
var counter = 0;
img.style.backgroundImage = 'url(' + imgArray[counter] + ')';

setInterval(function () {	
	counter++
	if (counter == imgArray.length)
		counter = 0;
	console.log(imgArray[counter] + " Counter : " + counter);
	img.style.backgroundImage = 'url(' + imgArray[counter] + ')';

	
},3000); 