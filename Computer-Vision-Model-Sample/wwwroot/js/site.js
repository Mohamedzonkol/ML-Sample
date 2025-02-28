// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<img id="imagePreview" src="data:image/png;base64,{{ Input.ImageData }}" />

<canvas id="boundingBoxCanvas"></canvas>

<script>
    const objects = {{ Input.Objects | Json.Serialize }};
    const img = document.getElementById('imagePreview');
    const canvas = document.getElementById('boundingBoxCanvas');
    const ctx = canvas.getContext('2d');

    img.onload = function () {
        canvas.width = img.width;
        canvas.height = img.height;

        ctx.drawImage(img, 0, 0);

        objects.forEach(obj => {
            ctx.strokeStyle = "red";
            ctx.lineWidth = 2;
            ctx.strokeRect(obj.Box.X, obj.Box.Y, obj.Box.Width, obj.Box.Height);
            ctx.fillStyle = "red";
            ctx.fillText(obj.Name, obj.Box.X, obj.Box.Y - 5);
        });
    };
</script>
