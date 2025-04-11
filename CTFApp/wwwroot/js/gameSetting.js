// Handle Import Form Submission
document.getElementById('importForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const fileInput = document.getElementById('fileInput');
    const responseDiv = document.getElementById('response');

    if (!fileInput.files.length) {
        responseDiv.innerHTML = '<div class="alert alert-danger">Please select a file.</div>';
        return;
    }

    const formData = new FormData();
    formData.append('file', fileInput.files[0]);

    try {
        const response = await fetch('/api/admin/import-users', {
            method: 'POST',
            body: formData,
            credentials: "include"
        });

        const result = await response.text();
        if (response.ok) {
            responseDiv.innerHTML = `<div class="alert alert-success">${result}</div>`;
        } else {
            responseDiv.innerHTML = `<div class="alert alert-danger">${result}</div>`;
        }
    } catch (error) {
        responseDiv.innerHTML = `<div class="alert alert-danger">Error: ${error.message}</div>`;
    }
});

// Handle Export Button Click
document.getElementById('exportButton').addEventListener('click', async () => {
    try {
        const response = await fetch('/api/admin/export-users');
        if (!response.ok) {
            throw new Error('Export failed: ' + response.statusText);
        }

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'users_export.json';
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);
    } catch (error) {
        alert('Error exporting users: ' + error.message);
    }
});