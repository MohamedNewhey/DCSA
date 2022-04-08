function myFunction() {
    var dots = document.getElementById("dots");
    var moreText = document.getElementById("more");
    var btnText = document.getElementById("myBtn");
  
    if (dots.style.display === "none") {
      dots.style.display = "inline";
      btnText.innerHTML = "المزيد...";
      moreText.style.display = "none";
    } else {
      dots.style.display = "none";
      btnText.innerHTML = "القليل";
      moreText.style.display = "inline";
    }
  }





  $(document).ready(function() {
    $(".down").click(function() {
         $('html, body').animate({
             scrollTop: $(".hadeer").offset().top
         }, 2);
     });


$("#nav li a").click(function()
{

$(this).addClass('active').parent().siblings().find('a').removeClass('active');
});

      /* side menu active link */
      $("#main-menu li a").click(function () {

          $(this).addClass('active2').parent().siblings().find('a').removeClass('active2');
          
      });

  /* side menu active link */
    });
    
function ChangeButtonText(li) {
    document.querySelector('#SearchButton').innerHTML = '<img class="Classification-img" src="/Content/Icons/levels.svg"/>' + li.innerText + '<img class="Classification-icondrop dropdown-toggle" src="/Content/Icons/Group 82.svg" />';

}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;

}