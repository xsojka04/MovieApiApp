<script lang="js">
import * as vNG from "v-network-graph"
import axios from "axios";
import dagre from "dagre/dist/dagre.min.js"
import router from '../router'
import { reactive } from "vue"

const nodeSize = 150
const nodeWidth = 200
// define basic look of the graph
const initialConfigs = vNG.defineConfigs({
    view: {
        scalingObjects: true,
        zoomEnabled: true
    },
    node: {
        selectable: false,
        draggable: true,
        normal: {
            type: node => node.type,
            // for type is "rect" -->
            width: nodeWidth,
            height: nodeSize,
            radius: 75,
            borderRadius: 10,
            // <-- for type is "rect"
            strokeWidth: 0,
            strokeColor: "#000000",
            strokeDasharray: "0",
            color: "#ffffff",
        },
        hover: {
            type: node => node.type,
            width: nodeWidth,
            height: nodeSize,
            radius: 75,
            strokeWidth: 2,
            strokeColor: "#000000",
            strokeDasharray: "0",
            color: "#ffffff",
        },
        label: {
            visible: false,
        },
        focusring: {
            visible: true,
            width: 4,
            padding: 3,
            color: "#eebb00",
            dasharray: "0",
        },
    },
    edge: {
        selectable: false,
        normal: {
            width: 3,
            color: "#aaaaaa",
            dasharray: "0",
            linecap: "butt",
            animate: false,
            animationSpeed: 50,
        },

        gap: 3,
        type: "straight",
    },
})
const nodes = reactive({})
const edges = reactive({})
const layouts = reactive({ nodes: {} })

export default {
    data() {
        return {
            configs: initialConfigs,
            nodes: nodes,
            edges: edges,
            layouts: layouts,
            maxEdges: 7
        }
    },
    setup() {
        return {}
    },
    mounted() {
        if (localStorage.configs) {
            this.configs = JSON.parse(localStorage.configs);
        } else { this.configs = initialConfigs }
        if (localStorage.nodes) {
            this.nodes = JSON.parse(localStorage.nodes);
        } else { this.configs = [] }
        if (localStorage.edges) {
            this.edges = JSON.parse(localStorage.edges);
        } else { this.configs = [] }
        if (localStorage.layouts) {
            this.layouts = JSON.parse(localStorage.layouts);
        } else { this.configs = { nodes: [] } }
    },
    methods: {
        addPerson(personId) {

            axios.get('/api/tmdb/get', { params: { url: `person%2F${personId}` } }).then(response => {
                const nodeName = `person${response.data.id}`
                this.nodes[nodeName] = { mediaType: 'person', name: response.data.name, type: 'circle', image: response.data.profile_path, text: response.data.name, id: response.data.id }
                this.layouts.nodes[nodeName] = { x: 0, y: 0 }
                this.addCredits(personId)
            }).catch(function () {
            })
        },
        addCredits(personId) {
            axios.get('/api/tmdb/get', { params: { url: `person%2F${personId}%2Fcombined_credits` } }).then(response => {

                const personNodeName = `person${response.data.id}`

                for (const credit of response.data.cast) {
                    Object.entries(this.nodes).forEach(([nodeId, node]) => {
                        const edgeNameToExistingMovie = `edge${personId}_${credit.id}`
                        if (node.name === credit.title && node.type === 'rectangle' && this.edges[edgeNameToExistingMovie] == undefined) {


                            this.edges[edgeNameToExistingMovie] = { source: nodeId, target: personNodeName }

                        }
                    })
                }
                let count = 0
                for (const credit of response.data.cast) {

                    const type = credit.media_type
                    const nodeName = `credits${type}_${credit.id}`
                    if (this.nodes[nodeName] != undefined) {
                        continue
                    }
                    count++
                    if (count > this.maxEdges) {
                        break
                    }
                    const text = credit.media_type == 'tv' ? credit.name : credit.title
                    this.nodes[nodeName] = { mediaType: credit.media_type, name: text, type: 'rectangle', image: credit.backdrop_path == undefined ? null : credit.backdrop_path, text: text, id: credit.id }
                    this.layouts.nodes[nodeName] = { x: 0, y: 0 }
                    const edgeName = `edge${personId}_${credit.id}`
                    this.edges[edgeName] = { source: personNodeName, target: nodeName }

                }
                // after the new nodes are added, compute new coordiantes of every node in the graph
                this.recomputeLayout()
            }).catch(function () {
            })

        },
        recomputeLayout() {
            const g = new dagre.graphlib.Graph()
            g.setGraph({
                rankdir: 'LR',
                nodesep: nodeSize / 3,
                edgesep: nodeSize,
                ranksep: nodeSize * 2,
                ranker: 'tight-tree'
            })
            g.setDefaultEdgeLabel(() => ({}))
            Object.entries(this.nodes).forEach(([nodeId,]) => {
                g.setNode(nodeId, { label: nodeId, width: nodeSize, height: nodeSize })
            })
            Object.values(this.edges).forEach(edge => {

                g.setEdge(edge.source, edge.target)
            })
            dagre.layout(g)

            g.nodes().forEach((nodeId) => {
                this.layouts.nodes[nodeId].x = g.node(nodeId).x
                this.layouts.nodes[nodeId].y = g.node(nodeId).y
            })
        },
        goTo(id, type) {
            if (type == 'person') {
                router.push({ name: 'personDetail', params: { id: id } })
            } else if (type == 'movie') {
                router.push({ name: 'movie', params: { id: id } })
            } else {
                router.push({ name: 'tv', params: { id: id } })
            }
        },
        reset() {
            this.nodes = {},
                this.edges = {},
                this.layouts = { nodes: {} }
        }
    },
    created() {
        if (this.$route.params.id != undefined) {
            this.addPerson(this.$route.params.id)
        }
    },
    watch: {
        configs: {
            handler(newValue) {
                localStorage.configs = JSON.stringify(newValue);
            },
            deep: true
        },
        nodes: {
            handler(newValue) {
                localStorage.nodes = JSON.stringify(newValue);
            },
            deep: true
        },
        edges: {
            handler(newValue) {
                localStorage.edges = JSON.stringify(newValue);
            },
            deep: true
        },
        layouts: {
            handler(newValue) {
                localStorage.layouts = JSON.stringify(newValue);
            },
            deep: true
        },
        $route() {
            if (this.$route.params.id != undefined) {
                this.addPerson(this.$route.params.id)
            }
        }
    }
}
</script>
<template>
    <h1>Poznávací síť</h1>
    <button @click="reset" class="reset-button">Reset</button>
    <span class="empty-web-info" v-if="Object.keys(nodes) == 0">Vaše poznávací síť je prázdná. Skuste vyhledat nějakého
        herce, nebo film</span>
    <v-network-graph :class="'graph' + (Object.keys(nodes) == 0 ? ' empty-web' : '')" :nodes="nodes" :edges="edges"
        :configs="configs" :layouts="layouts">
        <defs>
            <clipPath id="faceCircle" clipPathUnits="objectBoundingBox">
                <circle cx="0.5" cy="0.5" r="0.5" />
            </clipPath>
        </defs>

        <template #override-node="{ nodeId, scale, config, ...slotProps }">


            <image v-if="nodes[nodeId].type == 'rectangle'" class="node-picture" :x="-config.width * scale / 2"
                :y="-(config.height * scale) / 2" :width="config.width * scale" :height="config.height * scale"
                :href="nodes[nodeId].image != null ? `https://image.tmdb.org/t/p/w500${nodes[nodeId].image}` : require('@/assets/movie-placeholder.png')"
                preserveAspectRatio="xMidYMid slice" stroke-width="0" />
            <image v-else class="node-picture" :x="-config.radius * scale" :y="-config.radius * scale"
                preserveAspectRatio="xMidYMid slice" :width="config.radius * scale * 2" :height="config.radius * scale * 2"
                :xlink:href="nodes[nodeId].image != null ? `https://image.tmdb.org/t/p/w500${nodes[nodeId].image}` : require('@/assets/actor-placeholder.png')"
                clip-path="url(#faceCircle)" />

            <rect v-if="nodes[nodeId].type == 'rectangle'" :rx="config.borderRadius" :x="-config.width * scale / 2"
                :y="-config.height * scale / 2" :width="config.width * scale" :height="config.height * scale" fill="none"
                stroke="#808080" :stroke-width="config.strokeWidth * scale" v-bind="slotProps" />

            <circle v-else :rx="config.borderRadius" :x="-config.width * scale / 2" :y="-config.height * scale / 2"
                :r="config.radius * scale" fill="none" stroke="#808080" :stroke-width="config.strokeWidth * scale"
                v-bind="slotProps" />
            <foreignObject :y="config.height * scale / 3.5" :width="config.width * scale * 2"
                :height="config.height * scale" :x="-2 * (config.width * scale / 2)">
                <p @click="goTo(nodes[nodeId].id, nodes[nodeId].mediaType)" class="node-text title">
                    {{ nodes[nodeId].text }}
                </p>
            </foreignObject>
        </template>
    </v-network-graph>
</template>
<style>
.graph {
    width: 100vw;
    height: 80vh;
    border: 5px solid white;
    background: rgb(2, 0, 36);
    background: linear-gradient(43deg, rgba(2, 0, 36, 1) 0%, rgba(17, 41, 47, 1) 16%, rgba(18, 18, 46, 1) 42%, rgba(55, 33, 91, 1) 69%, rgba(11, 68, 40, 1) 95%);
}

.node-text {
    color: white;
    text-shadow: 0 0 5px black;
    font-size: 2.5rem;
    line-height: 2rem;
}

.node-text:hover {
    font-weight: 700;
    cursor: pointer;
    color: white;
}

.node-text-rectangle {
    filter: drop-shadow(-1px -2px 2px #555555);
}
</style>