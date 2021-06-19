function ValidateNewRequest()
{
   // $('#Company :selected').text();
    var selectedCompany = $('#Company :selected').val();
    var selectedRegion = $('#Region :selected').val();
    var selectedBranch = $('#Branch :selected').val();
    //alert(selectedCompany);
    //alert(selectedRegion);
    //alert(selectedBranch);
    if ((selectedCompany == null) || (selectedCompany == 0))
    {
        alert("Please Select Company");
        return false;

    }
    if ((selectedRegion == null) || (selectedRegion == 0))
    {
        alert("Please Select Region");
        return false;

    }
    if ((selectedBranch == null) || (selectedBranch == 0))
    {
        alert("Please Select Branch");
        return false;

    }
    if (selectedBranch != 'New') {
        alert("Please Select New Branch");
        return false;
    }
  
        return true;
   


}

function ValidateExistingRequest() {
    // $('#Company :selected').text();
    var selectedCompany = $('#Company :selected').val();
    var selectedRegion = $('#Region :selected').val();
    var selectedBranch = $('#Branch :selected').val();
    //alert(selectedCompany);
    //alert(selectedRegion);
    //alert(selectedBranch);
    if ((selectedCompany == null) || (selectedCompany == 0))
    {
        alert("Please Select Company");
        return false;

    }
    if ((selectedRegion == null) || (selectedRegion == 0))
    {
        alert("Please Select Region");
        return false;

    }
    if ((selectedBranch == null)|| (selectedBranch == 0))
    {
        alert("Please Select Branch");
        return false;

    }
    if (selectedBranch == 'New') {
        alert("Please Select an active Branch");
        return false;
    }

    return true;



}