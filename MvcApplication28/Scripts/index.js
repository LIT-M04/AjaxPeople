$(function () {
    $("#new-person").on('click', function () {
        $("#first-name").val('');
        $("#last-name").val('');
        $("#age").val('');
        $("#update-person").hide();
        $("#save-person").show();
        $(".modal").modal();
    });

    $("#save-person").on('click', function () {
        $("#save-person").button('loading');
        var person = {};
        person.firstName = $("#first-name").val();
        person.lastName = $("#last-name").val();
        person.age = $("#age").val();

        $.post("/home/addperson", person, function () {
            getPeople();
            $("#save-person").button('reset');
            $(".modal").modal('hide');
        });
    });

    $("table").on('click', '.delete', function () {
        var personId = $(this).data('person-id');
        if (!confirm("Are you sure?")) {
            return;
        }
        $.post('/home/delete', { id: personId }, getPeople);
    });

    $("table").on('click', '.edit', function () {
        var personId = $(this).data('person-id');
        $.get('/home/getperson', { id: personId }, function (result) {
            $("#first-name").val(result.person.FirstName);
            $("#last-name").val(result.person.LastName);
            $("#age").val(result.person.Age);
            $(".modal").data('person-id', result.person.Id);
            $("#update-person").show();
            $("#save-person").hide();
            $(".modal").modal();
        }).fail(function(error) {
            $("#error").html(error.responseText);
        });
    });

    $("#update-person").on('click', function() {
        $("#update-person").button('loading');
        var person = {};
        person.firstName = $("#first-name").val();
        person.lastName = $("#last-name").val();
        person.age = $("#age").val();
        person.id = $(".modal").data('person-id');
        $.post("/home/updateperson", person, function() {
            getPeople();
            $(".modal").modal('hide');
            $("#update-person").button('reset');
        });
    });

    function getPeople() {
        $.get("/home/getpeople", function (result) {
            $("table tr:gt(0)").remove();
            result.people.forEach(function (person) {
                $("table").append("<tr><td>" + person.FirstName + "</td><td>" +
                    person.LastName + "</td><td>" + person.Age + "</td><td>" +
                    "<button class='btn btn-warning edit' data-person-id='" + person.Id + "'>Edit</button>" +
                    " <button class='btn btn-danger delete' data-person-id='" + person.Id + "'>Delete</button></td>");
            });
        });
    }
});

