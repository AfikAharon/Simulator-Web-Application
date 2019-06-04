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

function drawCircleAndLine(prevLon, prevLat, firstIterFlag, ctx, xml) {
    if (firstIterFlag) {
        var lon = (prevLon + 180) * (window.innerWidth / 360);
        var lat = (prevLat + 90) * (window.innerHeight / 180);
        drawCircle(ctx, lon, lat);
        return { Lon: lon, Lat: lat }
    }
    var lon = getLonFromXml(xml);
    lon = (lon + 180) * (window.innerWidth / 360);
    var lat = getLatFromXml(xml);
    lat = (lat + 90) * (window.innerHeight / 180);
    drawLine(ctx, prevLon, prevLat, lon, lat);
    return {Lon: lon, Lat:lat}
}

function getLonFromXml(xml) {
    var xmlDoc = $.parseXML(xml),
    $xml = $(xmlDoc),
    xmlLon = $xml.find("Lon").text();
    var tempLon = parseFloat(xmlLon);
    return tempLon;
}

function getLatFromXml(xml) {
    var xmlDoc = $.parseXML(xml),
    $xml = $(xmlDoc),
    xmlLat = $xml.find("Lat").text();
    var tempLat = parseFloat(xmlLat);
    return tempLat;
}