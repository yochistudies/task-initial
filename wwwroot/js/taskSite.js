const url = '/Task';
let tasks = [];

function getTasks() {
    fetch(url)
        .then(response => response.json())
        .then(data => _displayTasks(data))
        .catch(error => console.error('Unable to get tasks.', error));
}

function addTask() {
    const addNameTextbox = document.getElementById('add-name');

    const task = {
        status: false,
        name: addNameTextbox.value.trim()
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(task)
    })
        .then(response => response.json())
        .then(() => {
            getTasks();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add task.', error));
}

function deleteTask(id) {
    fetch(`${url}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to delete task.', error));
}

function displayEditForm(id) {
    const task = tasks.find(task => task.id === id);

    document.getElementById('edit-name').value = task.name;
    document.getElementById('edit-id').value = task.id;
    document.getElementById('edit-status').checked = task.status;
    document.getElementById('editForm').style.display = 'block';
}

function updateTask() {
    const taskId = document.getElementById('edit-id').value;
    const task = {
        id: parseInt(taskId, 10),
        status: document.getElementById('edit-status').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${url}/${taskId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(task)
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to update task.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(taskCount) {
    const name = (taskCount === 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${taskCount} ${name}`;
}

function _displayTasks(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(task => {
        let statusCheckbox = document.createElement('input');
        statusCheckbox.type = 'checkbox';
        statusCheckbox.disabled = true;
        statusCheckbox.checked = task.status;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${task.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteTask(${task.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(statusCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(task.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}