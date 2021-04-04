// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function toggleUserDetails() {
    var newUserDetails = document.getElementById("newUserDetails");
    var findUserDetails = document.getElementById("findUserDetails");
    newUserDetails.hidden = !newUserDetails.hidden;
    findUserDetails.hidden = !findUserDetails.hidden;
}

function addValue(sourceId, destinationId, delimeter, uniqueOnly) {
    var sourceEl = document.getElementById(sourceId);
    var destinationEl = document.getElementById(destinationId);

    if (sourceEl.value=="") return;
      
    if (destinationEl.value.length>0) {
         if (uniqueOnly==true && destinationEl.value.indexOf(sourceEl.value) !== -1
         ) {
            return;
         } else {
         destinationEl.value= destinationEl.value + delimeter + sourceEl.value;    
         }
    } else {
            destinationEl.value = sourceEl.value;   
    }
}
function removeValue(sourceId, destinationId, delimeter, uniqueOnly) {
    var sourceEl = document.getElementById(sourceId);
    var destinationEl = document.getElementById(destinationId);

    if (sourceEl.value=="") return;
      
    if (destinationEl.value.length>0) {
            var dest = destinationEl.value.split(delimeter);
            var new_dest = "";
            for (var i = 0; i < dest.length; i++) {
                if (dest[i] != sourceEl.value) {
                    if (! (uniqueOnly==true && new_dest.indexOf(dest[i])!== -1)) {
                        if (new_dest.length > 0) {
                            new_dest = new_dest + delimeter + dest[i];
                        } else {
                          new_dest = dest[i];
                        }
                    }
                }
            } 
    destinationEl.value = new_dest;
    }
    
} 

function clearValue(Id) {
    var el = document.getElementById(Id);
    el.value = "";
}
function addJSONList(nameId, valueId, jsonList) {
    var name = document.getElementById(nameId);
    var value = document.getElementById(valueId);
    var jlist = document.getElementById(jsonList);
    var jo = null;
    
    if (jlist.value.length>0){
       jo = JSON.parse(jlist.value);  
    } else {
       jo = JSON.parse("{}");    
    }

    if (name.value.length>0){
        if (value.value.length>0){
            jo[name.value] = value.value; 
        }
    }

    jlist.value = JSON.stringify(jo);
}

function addTransform() {
    addJSONList ("selectParam","selectId","transform_parameters")
}

function clearTransform() {
   clearValue("transform_parameters");
}

function showLocationType() {

    var locType = document.getElementById("locSelect");
    var locAddressPostcode = document.getElementById("locAddressPostcode");
    var locNatGrid = document.getElementById("locNatGrid");
    var locLatLong = document.getElementById("locLatLong");
    var locMapRef = document.getElementById("locMapRef");

    var  locTypeSelected = locType.options [locType.selectedIndex].value;
    
    locNatGrid.hidden=true;
    locLatLong.hidden=true;
    locMapRef.hidden=true;
    locAddressPostcode.hidden=true;

    if (locTypeSelected=="locNatGrid"){locNatGrid.hidden=false}
    if (locTypeSelected=="locLatLong"){locLatLong.hidden=false}
    if (locTypeSelected=="locMapRef"){locMapRef.hidden=false}
    if (locTypeSelected=="locAddressPostcode"){locAddressPostcode.hidden=false}
}

function updateGoogleMap() {
    var googlemap = document.getElementById("googlemap");
    var locLat = document.getElementById("locLatitude");
    var locLon = document.getElementById("locLongitude");
    var google1 = "https://maps.google.com/maps?width=100%&height=600&hl=en"
    var google2 ="ie=UTF8&t=&z=14&iwloc=B&output=embed"
    
    if (locLat && locLat.value && locLong && locLong.value) {
        var googleloc = "?q=" + locLat.value +"," + locLon.value
        googlemap.src = google1 + googleloc + google2
    } 

    // googlemap.src="https://maps.google.com/maps?width=100%&height=600&hl=en&q=Malet%20St%2C%20London%20WC1E%207HU%2C%20United%20Kingdom+(Your%20Business%20Name)&ie=UTF8&t=&z=14&iwloc=B&output=embed";

}

function dodrop(event) {
  var dt = event.dataTransfer;
  var files = dt.files;
  var uf = document.getElementById("uploadFiles");
  uf.files=files;
  updateSelectedFiles();
}

function updateSelectedFiles(maxfiles){
    
    var uf = document.getElementById("uploadFiles");
    var ol = document.getElementById("selectedFiles");
    var lmd = document.getElementById("LastModifiedDates");
    
    var dates='';
    ol.innerHTML = ""; 
    var d;

    for (var i = 0; i < uf.files.length; i++) {
        if (maxfiles > 0 && i >= maxfiles) {
            break;
        }
        var file = uf.files[i];
        var lastModified = file.lastModified || file.lastModifiedDate;
        if (Number.isInteger(lastModified)){
        d = new Date(lastModified);
        } else {
        d = new Date(Date.parse(lastModified));    
        }
        var fdetails = file.name  + ',' + file.size + ' (bytes),' + d.toUTCString() +' (lastmodified)';
        li = document.createElement('li');
        li.appendChild(document.createTextNode(fdetails));
        ol.appendChild(li);
        if (i>0) dates += ';';
        dates += d.toUTCString();  
    }

    lmd.value = dates;
    }
    
    Number.isInteger = Number.isInteger || function(value) {
        return typeof value === 'number' && 
          isFinite(value) && 
          Math.floor(value) === value;
      };
      