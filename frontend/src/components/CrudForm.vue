<template>
  <div class="container">
    <h1 class="text-center">Administrador de eventos</h1>
    <div class="row mt-5">
      <div class="col-md-6">
        <div class="card">
          <div class="alert alert-danger m-3" role="alert" v-if="error">
            {{ error }}
          </div>
          <form class="row card-body" @submit.prevent="save">
            <div class="col-md-6 mb-3">
              <label for="date" class="form-label">Fecha</label>
              <input
                type="date"
                class="form-control"
                id="date"
                name="date"
                required
                v-model="event.date"
              />
            </div>
            <div class="col-md-6 mb-3">
              <label for="price" class="form-label">Precio</label>
              <input
                type="text"
                class="form-control"
                id="price"
                name="price"
                required
                v-model="event.price"
              />
            </div>
            <div class="col-md-12 mb-3">
              <label for="location" class="form-label">Lugar</label>
              <input
                type="text"
                class="form-control"
                id="location"
                name="location"
                required
                v-model="event.location"
              />
            </div>
            <div class="col-md-12 mb-3">
              <label for="description" class="form-label">Descripción</label>
              <input
                type="text"
                class="form-control"
                id="description"
                name="description"
                required
                v-model="event.description"
              />
            </div>
            <div class="col-md-1 mb-3">
              <button class="btn btn-primary" type="submit">
                {{ btnText }}
              </button>
            </div>
          </form>
        </div>
      </div>
      <div class="col-md-6 card table-responsive">
        <table class="table">
          <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">Fecha</th>
              <th scope="col">Lugar</th>
              <th scope="col">Precio</th>
              <th scope="col">Descripcion</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, key) in items" :key="key">
              <th scope="row">
                {{ key + 1 }}
              </th>
              <td>
                {{ item.date }}
              </td>
              <td>
                {{ item.location }}
              </td>
              <td>
                {{ item.price }}
              </td>
              <td>
                {{ item.description }}
              </td>
              <td>
                <button
                  type="button"
                  class="btn btn-sm btn-primary mx-1"
                  @click="update(item)"
                >
                  <i class="fa fa-edit"></i>
                </button>
                <button
                  type="button"
                  class="btn btn-sm btn-danger mx-1"
                  @click="deleteItem(item.id)"
                >
                  <i class="fa fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
        <nav aria-label="Page navigation example">
          <ul class="pagination">
            <li
              :class="`page-item ${p === currentPage ? 'active' : ''}`"
              v-for="(p, key) in totalPages"
              :key="key"
              @click="setCurrentPage(p)"
            >
              <a class="page-link" href="#">{{ p }}</a>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
const API_URL = process.env.VUE_APP_API_URL;
export default {
  name: "CrudForm",
  data() {
    return {
      items: [],
      totalPages: 0,
      pageSize: 5,
      currentPage: 1,
      event: {
        date: "",
        price: 0,
        location: "",
        description: "",
      },
      error: "",
      btnText: "Guardar",
    };
  },
  methods: {
    get() {
      const params = this.getParams();
      axios
        .get(`${API_URL}/Events`, { params })
        .then((res) => {
          const { data } = res;
          this.items = data.data;
          this.totalPages = data.totalPages;
        })
        .catch((res) => {
          console.error(res);
        });
    },
    getParams() {
      const params = {
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
      };

      return params;
    },
    setCurrentPage(value) {
      this.currentPage = value;
      this.get();
    },
    save() {
      this.error = "";

      if (this.event.id) {
        axios
          .put(`${API_URL}/Events/${this.event.id}`, this.event)
          .then(() => {
            this.get();
            this.event = {
              date: "",
              price: 0,
              location: "",
              description: "",
            };
            this.btnText = "Guardar";
            alert("Evento actualizado !!");
          })
          .catch((res) => {
            const {
              response: { data },
            } = res;
            this.error = data.title;
          });

        return;
      }
      axios
        .post(`${API_URL}/Events`, this.event)
        .then(() => {
          this.get();
          this.event = {
            date: "",
            price: 0,
            location: "",
            description: "",
          };
          this.btnText = "Guardar";
          alert("Evento ingresado !!");
        })
        .catch((res) => {
          const {
            response: { data },
          } = res;
          this.error = data.title;
        });
    },
    update(item) {
      this.event = Object.assign({}, item);
      this.btnText = "Actualizar";
    },
    deleteItem(id) {
      if (window.confirm("¿Estás seguro desea eliminar el evento?")) {
        axios
          .delete(`${API_URL}/Events/${id}`)
          .then(() => {
            this.get();
            alert("Evento eliminado !!");
          })
          .catch((res) => {
            const {
              response: { data },
            } = res;
            this.error = data.title;
          });
      }
    },
  },
  created() {
    this.get();
  },
};
</script>
