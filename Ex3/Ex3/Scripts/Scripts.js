DrawCircle = function (ctx, x, y) {
    ctx.beginPath();
    ctx.arc(x, y, 4, 0, 2 * Math.PI);
    ctx.lineWidth = 2;
    ctx.lineStyle = 'blue';
    ctx.fillStyle = 'red';
    ctx.fill();
    ctx.stroke();
}