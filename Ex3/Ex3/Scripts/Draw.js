function drawCircle(ctx, x, y) {
    ctx.beginPath();
    ctx.arc(x, y, 4, 0, 2 * Math.PI);
    ctx.lineWidth = 2;
    ctx.lineStyle = 'blue';
    ctx.fillStyle = 'red';
    ctx.fill();
    ctx.stroke();
}

function drawLine(ctx, startX,startY, endX, endY) {
    ctx.beginPath();
    ctx.lineWidth = 2;
    ctx.strokeStyle = 'red';
    ctx.moveTo(startX, startY);
    ctx.lineTo(endX, endY);
    ctx.stroke();
}

function drawCircleAndLine(prevLon, prevLat, firstIterFlag, ctx,xml) {
    var xmlDoc = $.parseXML(xml),
    $xml = $(xmlDoc),
    xmlLon = $xml.find("Lon").text();
    var lon = parseFloat(xmlLon);
    lon = (lon + 180) * (window.innerWidth / 360);
    xmlLat = $xml.find("Lat").text();
    var lat = parseFloat(xmlLat);
    lat = (lat + 90) * (window.innerHeight / 180);
    // check if it's the first iteration
    if (firstIterFlag) {
        drawCircle(ctx, lon, lat);
    } else {
        drawLine(ctx, prevLon, prevLat, lon, lat);
    }
    return {Lon: lon, Lat:lat}
}