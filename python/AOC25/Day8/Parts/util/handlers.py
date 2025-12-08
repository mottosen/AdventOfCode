import re
from math import sqrt


def line_2_vertice(line):
    return {
        "pos": tuple(map(int, re.findall(r"\d+", line))),
        "circuit": 0,
    }


def edge_len(v1, v2):
    return sqrt((v1[0] - v2[0]) ** 2 + (v1[1] - v2[1]) ** 2 + (v1[2] - v2[2]) ** 2)


def build_graph(input):
    vertices = [line_2_vertice(line) for line in input]
    edges = []

    for i, v1 in enumerate(vertices):
        for v2 in vertices[i + 1 :]:
            edges.insert(0, (edge_len(v1["pos"], v2["pos"]), (v1, v2)))

    edges.sort(key=lambda edge: edge[0])

    return vertices, edges


def handle_edge(circuits, counter, v1, v2):
    if v1["circuit"] == 0:
        if v2["circuit"] == 0:
            # both vertices are new
            circuits[counter] = [v1, v2]
            v1["circuit"] = counter
            v2["circuit"] = counter
            counter += 1
        else:
            # vertice v1 is new
            v2_circuit = v2["circuit"]
            circuits[v2_circuit].insert(0, v1)
            v1["circuit"] = v2_circuit
    else:
        if v1["circuit"] == v2["circuit"]:
            # both vertices in same circuit already
            return counter
        elif v2["circuit"] == 0:
            # vertice v2 is new
            v1_circuit = v1["circuit"]
            circuits[v1_circuit].insert(0, v2)
            v2["circuit"] = v1_circuit
        else:
            # both vertices are known, merge circuits
            v1_circuit, v2_circuit = v1["circuit"], v2["circuit"]
            for v in circuits[v2_circuit]:
                v["circuit"] = v1_circuit

            merge_circuit = circuits.pop(v2_circuit)
            circuits[v1_circuit] += merge_circuit

    return counter
