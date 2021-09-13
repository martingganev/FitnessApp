var currentTab = 0;
function initializeRegisterScript(){
  currentTab = 0;
}

function hideBtn(buttonId){
  document.getElementById(buttonId).style.display = "none";
}

function hideSubmitBtn(){
  hideBtn("submitBtn");
}

function showBtn(buttonId){
  document.getElementById(buttonId).style.display = "inline";
}

function showTab(n) {
  var x = document.getElementsByClassName("tab");
  x[n].style.display = "block";
  if (n == 0) {
    hideBtn("prevBtn");
  } else {
    showBtn("prevBtn");
  }
  if (n == (x.length - 1)) {
    hideBtn("nextBtn");
    showBtn("submitBtn");
  } else {
    hideBtn("submitBtn");
    showBtn("nextBtn");
  }
  fixStepIndicator(n)
}

function nextPrev(n) {
  var x = document.getElementsByClassName("tab");
  if (n == 1 && !validateForm()) return false;
  x[currentTab].style.display = "none";
  currentTab = currentTab + n;
  if (currentTab >= x.length) {
    //document.getElementById("regForm").submit();
    return false;
  }
  showTab(currentTab);
}

function validateForm() {
  var x, y, i, valid = true;
  x = document.getElementsByClassName("tab");
  y = x[currentTab].getElementsByClassName("form-control");
  for (i = 0; i < y.length; i++) {
    if (y[i].value == "") {
      y[i].className += " invalid";
      valid = false;
    } else{
      y[i].className -= " invalid";
    }
  }
  if(currentTab == 1){
    let radioInputs = document.getElementsByClassName("form-check-input");
    if(radioInputs[0].checked === false && radioInputs[1].checked === false){
      document.getElementsByClassName("form-check-label")[0].style.color = "red";
      document.getElementsByClassName("form-check-label")[1].style.color = "red";
      valid = false;
    } else {
      document.getElementsByClassName("form-check-label")[0].style.color = "";
      document.getElementsByClassName("form-check-label")[1].style.color = "";
    }
  }
  if (valid) {
    document.getElementsByClassName("step")[currentTab].className += " finish";
  }
  return valid;
}

function fixStepIndicator(n) {
  var i, x = document.getElementsByClassName("step");
  for (i = 0; i < x.length; i++) {
    x[i].className = x[i].className.replace(" active", "");
  }
  x[n].className += " active";
}