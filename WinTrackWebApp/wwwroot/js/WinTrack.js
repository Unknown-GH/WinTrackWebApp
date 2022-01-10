const WinTrack = (() => {
    const configMap = {
        serverUrl: ""
    }
    const init = (serverUrl) => {
        configMap.serverUrl = serverUrl;
    }

    const switchData = () => {
        $("#demoToggle").prop("disabled", true);
        $("#trackToggle").prop("disabled", true);
        $.ajax({
            url: `${configMap.serverUrl}/home/switchdata`,
            type: "POST"
        }).then(() => {
            if ($("#demoToggle").prop("checked")) {
                if ($(".track").attr("src") == "/images/OffLeft.png") { $(".track").attr("src", "/images/OnLeft.png"); }
                else if ($(".track").attr("src") == "/images/OffRight.png") { $(".track").attr("src", "/images/OnRight.png"); }
            }
            else {
                let trackStatus = "";
                $.ajax({
                    url: `${configMap.serverUrl}/home/getconnection`,
                    type: "GET"
                }).then(result1 => {
                    if (result1) { trackStatus += "On" }
                    else { trackStatus += "Off" }

                    $.ajax({
                        url: `${configMap.serverUrl}/home/gettrackstatus`,
                        type: "GET"
                    }).then(result2 => {
                        if (result2) { trackStatus += "Right" }
                        else { trackStatus += "Left" }
                        $(".track").attr("src", "/images/" + trackStatus + ".png");
                    });

                });


            }
            setTimeout(() => {
                $("#demoToggle").prop("disabled", false);
                $("#trackToggle").prop("disabled", false);
            }, 2000);

        });
    }

    const switchTrack = () => {
        $("#trackToggle").prop("disabled", true);

        if ($(".track").attr("src").includes("Off")) {
            setTimeout(() => {
                $("#trackToggle").prop("checked", !$("#trackToggle").prop("checked"));
            }, 100);
        }

        $.ajax({
            url: `${configMap.serverUrl}/home/switchtrack`,
            type: "POST"
        }).then(() => {
            if ($(".track").attr("src") == "/images/OnLeft.png") { $(".track").attr("src", "/images/OnRight.png"); }
            else if ($(".track").attr("src") == "/images/OnRight.png") { $(".track").attr("src", "/images/OnLeft.png"); }
            setTimeout(() => { $("#trackToggle").prop("disabled", false); }, 500);
        });
    }

    const toggleTrackWindow = () => {
        if ($(".track-window:visible").length) { $(".track-window").hide(); }
        else { $(".track-window").show(); }
    }

    return {
        init: init,
        switchData: switchData,
        switchTrack: switchTrack,
        toggleTrackWindow: toggleTrackWindow
    }
})();